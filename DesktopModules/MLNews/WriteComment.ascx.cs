using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DotNetNuke.News
{
    public partial class WriteComment : System.Web.UI.UserControl
    {
        private int cIntNewsID;
        public int PIntNewsID
        {
            get { return cIntNewsID; }
            set { cIntNewsID = value; }
        }

        private string GenerateSecureText()
        {
            string fStrSecure = "";
            Random fRand = new Random();
            for (int i = 0; i < 5; i++)
            {
                fStrSecure += fRand.Next(0, 9).ToString();
            }
            return fStrSecure;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string fStrSecure = GenerateSecureText();
                Session["SecureText"] = fStrSecure;
                txtSecure.Text = fStrSecure;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSecureKey.Text != (string)Session["SecureText"])
                {
                    lblNonValid.Visible = true;
                    return;
                }
                if (PIntNewsID != null)
                {
                    NewsController fNewsCont = new NewsController();
                    CommentInfo fComment = new CommentInfo();
                    fComment.NewsID = PIntNewsID;
                    fComment.Headline = txtHeadline.Text;
                    fComment.Content = txtContent.Text;
                    fComment.AuthorEmail = txtEmail.Text;
                    fComment.Author = txtName.Text;
                    fComment.CreatedDate = DateTime.Now;
                    fComment.Status = 0;
                    fComment.ClientIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(fComment.ClientIPAddress))
                    {
                        string[] ipRange = fComment.ClientIPAddress.Split(',');
                        fComment.ClientIPAddress = ipRange[0].Trim();
                    }
                    else
                    {
                        fComment.ClientIPAddress = Request.ServerVariables["REMOTE_ADDR"];
                    }
                    fNewsCont.InsertComment(fComment);
                    string fStrSecure = GenerateSecureText();
                    Session["SecureText"] = fStrSecure;
                    txtSecure.Text = fStrSecure;
                    lblNonValid.Visible = false;
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}