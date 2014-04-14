using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ARK.Model.XML;

namespace ARK.Model
{
    public class Members
    {
        private List<Member> _memberList;

        public List<Member> GetAllMembers()
        {
            return new List<Member>(_memberList);
        }

        public void UpdateFromXml(FtpInfo ftpInfo, string ftpPath)
        {
            UriBuilder ub = new UriBuilder("ftp", ftpInfo.HostName, ftpInfo.Port, ftpPath);
            NetworkCredential ftpCreds = new NetworkCredential(ftpInfo.Username, ftpInfo.Password);

            string xml = XMLParser.DlToMemFromFtp(ub.Uri, ftpCreds);
            var data = XMLParser.ParseXML<XMLMembers.dataroot>(xml);

            _memberList = data.activeMembers.Select(x => new Member(x)).ToList();
        }
    }
}
