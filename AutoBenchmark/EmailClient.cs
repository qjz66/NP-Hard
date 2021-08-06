using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using S22.Imap;
using SevenZipExtractor;


namespace Analyzer {
    public class EmailFetcher {
        // [NonBlocking]
        public static bool fetch() {
            try {
                return checkUnseenMails();
            } catch (Exception e) {
                Util.log("[error] fetch fail due to " + e.Message);
                return false;
            }
        }


        static bool checkUnseenMails() {
            Util.log("[info] query unseen emails");
            using (ImapClient client = createImapClient()) {
                bool updated = false;
                IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());
                foreach (var uid in uids) {
                    MailMessage msg = client.GetMessage(uid, FetchOptions.Normal, false);
                    if (msg.Subject.StartsWith(EmailCfg.SubjectFilter)) {
                        Util.log("[info] handle " + msg.Subject);
                        updated |= handleMessage(msg);
                        client.AddMessageFlags(uid, null, MessageFlag.Seen);
                        //client.DeleteMessage(uid);
                    } else {
                        Util.log("[info] ignore " + msg.Subject);
                    }
                }
                return updated;
            }
        }

        static bool handleMessage(MailMessage msg) {
            DateTime now = DateTime.Now;

            Submission s = new Submission();
            s.problem = msg.Subject.subStr(EmailCfg.ProblemIndexBegin, msg.Subject.IndexOf('-'));
            if (!BenchmarkCfg.rank.problems.ContainsKey(s.problem) || !BenchmarkCfg.Checkers.ContainsKey(s.problem)) {
                Util.log("[error] problem not available");
                return false;
            }

            s.author = msg.Subject.Substring(msg.Subject.IndexOf('-') + 1);
            s.email = msg.From.Address;
            s.date = Util.friendlyDateTime(now);

            string dirPath = Path.Combine(s.problem, CommonCfg.SolverSubDir, s.author + Util.compactDateTime(now));
            Func<string, string> detectExe = (string fileName) => {
                string filePath = Path.Combine(dirPath, fileName);
                if (!fileName.EndsWith(".exe")) { return filePath; }
                if (s.exePath != null) { Util.log("[warning] multiple executable detected"); }
                return s.exePath = filePath;
            };
            try {
                Directory.CreateDirectory(dirPath);
                foreach (var file in msg.Attachments) {
                    if (CommonCfg.ZipFileExts.Contains(Path.GetExtension(file.Name))) {
                        using (ArchiveFile archiveFile = new ArchiveFile(file.ContentStream)) {
                            foreach (Entry entry in archiveFile.Entries) {
                                if (entry.Size > EmailCfg.MaxFileByteSize) { Util.log("[warning] skip file larger than 4MB"); continue; }
                                entry.Extract(detectExe(entry.FileName));
                            }
                        }
                    } else {
                        file.save(detectExe(file.Name));
                    }
                }
            } catch (Exception e) {
                Util.log("[error] save attachment fail due to " + e.Message);
                return false;
            }

            if (s.exePath != null) {
                Benchmark.push(s); // EXT[szx][1]: reply "your submission has been received".
                return true;
            }
            Util.log("[error] no executable found"); // EXT[szx][1]: reply "no executable found in your submission".
            return false;
        }

        static ImapClient createImapClient() {
            return new ImapClient(EmailCfg.ExqqMail.Imap, EmailCfg.ImapSslPort, EmailCfg.Username, EmailCfg.Password, AuthMethod.Login, true);
        }
    }


    public class StdSmtp {
        public static void send(string toAddress, string subject, string body) {
            using (SmtpClient client = new SmtpClient(EmailCfg.HustMail.Smtp)) {
                //client.Port = EmailCfg.SmtpSslPort;
                //client.EnableSsl = true;
                client.Credentials = new NetworkCredential(EmailCfg.Username, EmailCfg.Password);

                using (MailMessage message = new MailMessage()) {
                    message.From = new MailAddress(EmailCfg.MyAddress);
                    message.To.Add(toAddress);
                    message.CC.Add(EmailCfg.CcAddress);
                    message.SubjectEncoding = Encoding.UTF8;
                    message.Subject = subject;
                    message.BodyEncoding = Encoding.UTF8;
                    message.Body = body;

                    client.Send(message);
                }
            }
        }
    }
}
