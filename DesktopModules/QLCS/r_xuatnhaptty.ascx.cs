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
using DotNetNuke.Services.Localization;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.QLCS
{
    public partial class r_xuatnhaptty : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
        private void BindControls()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
            txtFromDate.Text = "01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ci = System.Globalization.CultureInfo.CreateSpecificCulture(UserInfo.Profile.PreferredLocale);
            try
            {
                if (!Page.IsPostBack)
                {
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string filename = "TTY";
                string tieude = "";
                string strSQL = "QLCS_BCTK_XuatNhapTTY";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                filename += txtFromDate.Text + "___" + txtToDate.Text + ".xls";
                filename = filename.Replace("/", "_");
                tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                //Xem TTY nao co tong != 0 thi de lai
                DataTable tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by IDVatTu Asc");
                int countTTY = tblTTY.Rows.Count;

                //decimal[] tong = new decimal[countTTY * 3];
                ArrayList tong = new ArrayList();
                ArrayList tong1 = new ArrayList();
                for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                int tonIndex1 = countTTY * 2;
                foreach (DataRow r in tblTTY.Rows)
                {
                    tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                    tonIndex1++;
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                        j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                        j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                        {
                            tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                        }
                        j++;
                    }
                }
                for (int k = countTTY - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                    {
                        tblTTY.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countTTY + countTTY);
                        tong.RemoveAt(k + countTTY);
                        tong.RemoveAt(k);
                        tong1.RemoveAt(k);
                        countTTY--;
                    }
                }
                //int countTTY = tblTTY.Rows.Count;

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.ms-excel";
                string s = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></head><body width='100%' style='text-align:center;font-family:Times New Roman;'><br/><center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>";
                s += "<table border='1'>";
                s += @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td rowspan=2>Ngày</td>
                          <td colspan=" + countTTY.ToString() + @">Nhập (kg)</td>
                          <td colspan=" + countTTY.ToString() + @">Xuất (kg)</td>
                          <td colspan=" + countTTY.ToString() + @">Tồn (kg)</td>
                          <td rowspan=2>Ghi chú</td>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>";
                for (int i = 0; i < 3; i++)
                {
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        s += "<td>" + r["TenVatTu"].ToString() + "</td>";
                    }
                }
                s += "</tr>";
                s += "<tr><td>ĐK</td>";
                for (int i = 0; i < countTTY * 2; i++)
                {
                    s += "<td></td>";
                }
                //decimal[] tong = new decimal[countTTY * 3];
                //int tonIndex = countTTY * 2;
                foreach (DataRow r in tblTTY.Rows)
                {
                    s += "<td>" + Config.ToXVal1(dt.Rows[0][r["IDVatTu"].ToString()]) + "</td>";
                    //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    //tonIndex++;
                }
                s += "<td></td></tr>";
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    s += "<tr>";
                    s += "<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>";
                    //int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        s += "<td>" + Config.ToXVal(r["Nhap" + rTTY["IDVatTu"].ToString()]) + "</td>";
                        //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                        //j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        s += "<td>" + Config.ToXVal(r["Xuat" + rTTY["IDVatTu"].ToString()]) + "</td>";
                        //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                        //j++;
                    }
                    //int j = countTTY * 2;
                    int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                        {
                            s += "<td>" + Config.ToXVal1(tong1[j]) + "</td>";
                        }
                        else
                        {
                            s += "<td>" + Config.ToXVal1(r[rTTY["IDVatTu"].ToString()]) + "</td>";
                            tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                        }
                        j++;
                    }
                    s += "<td></td></tr>";
                }

                //Tính tổng
                s += "<tr style='font-weight:bold; vertical-align:middle;'><td>T.C</td>";
                for (int i = 0; i < countTTY * 3; i++)
                {
                    s += "<td>" + Config.ToXVal1(tong[i]) + "</td>";
                }
                s += "</tr>";
                s += "</table>";
                s += Config.Footer(UserInfo.Profile.City, UserInfo.FirstName + " " + UserInfo.LastName);
                s += "</body></html>";
                Response.Write(s);
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN");
                string tieude = "";
                string strSQL = "QLCS_BCTK_XuatNhapTTY";
                SqlParameter[] param = new SqlParameter[3];
                if (txtFromDate.Text == "")
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (txtToDate.Text == "")
                {
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                param[0] = new SqlParameter("@dFrom", txtFromDate.Text);
                param[1] = new SqlParameter("@dTo", txtToDate.Text);
                DateTime nextDateTo = DateTime.Parse(txtToDate.Text, ci).AddDays(1);
                param[2] = new SqlParameter("@nextDTo", nextDateTo.ToString("dd/MM/yyyy"));
                tieude += "<b>BẢNG THEO DÕI XUẤT, NHẬP THUỐC THÚ Y TỪ NGÀY " + txtFromDate.Text + " ĐẾN NGÀY " + txtToDate.Text + "</b>";
                DataTable dt = DotNetNuke.NewsProvider.DataProvider.SelectSP(strSQL, param);

                //Xem TTY nao co tong != 0 thi de lai
                DataTable tblTTY = DotNetNuke.NewsProvider.DataProvider.Select("select * from dnn_QLCS_VatTu where LoaiVatTu='TTY' and Active=1 order by IDVatTu Asc");
                int countTTY = tblTTY.Rows.Count;

                //decimal[] tong = new decimal[countTTY * 3];
                ArrayList tong = new ArrayList();
                ArrayList tong1 = new ArrayList();
                for (int l = 0; l < countTTY * 3; l++) tong.Add(new decimal(0));
                for (int l1 = 0; l1 < countTTY; l1++) tong1.Add(new decimal(0));
                int tonIndex1 = countTTY * 2;
                foreach (DataRow r in tblTTY.Rows)
                {
                    tong[tonIndex1] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    tong1[tonIndex1 - (countTTY * 2)] = tong[tonIndex1];
                    tonIndex1++;
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                        j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        tong[j] = ((decimal)tong[j]) + Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                        j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        if (r[rTTY["IDVatTu"].ToString()] != DBNull.Value)
                        {
                            tong[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                        }
                        j++;
                    }
                }
                for (int k = countTTY - 1; k >= 0; k--)
                {
                    if (((decimal)tong[k]) == 0 && ((decimal)tong[k + countTTY]) == 0 && ((decimal)tong[k + countTTY + countTTY]) == 0)
                    {
                        tblTTY.Rows.RemoveAt(k);
                        tong.RemoveAt(k + countTTY + countTTY);
                        tong.RemoveAt(k + countTTY);
                        tong.RemoveAt(k);
                        tong1.RemoveAt(k);
                        countTTY--;
                    }
                }
                //int countTTY = tblTTY.Rows.Count;

                string s = "<center style='font-weight:bold;font-size:15pt;'>" + tieude + "</center><br/>";
                s += "<table border='1' class='mGrid' id='thongke'><thead>";
                s += @"<tr style='font-weight:bold; vertical-align:middle;'>
                          <td rowspan=2>Ngày</td>
                          <td colspan=" + countTTY.ToString() + @">Nhập (kg)</td>
                          <td colspan=" + countTTY.ToString() + @">Xuất (kg)</td>
                          <td colspan=" + countTTY.ToString() + @">Tồn (kg)</td>
                          <td rowspan=2>Ghi chú</td>
                         </tr>
                         <tr style='font-weight:bold; vertical-align:middle;'>";
                for (int i = 0; i < 3; i++)
                {
                    foreach (DataRow r in tblTTY.Rows)
                    {
                        s += "<td>" + r["TenVatTu"].ToString() + "</td>";
                    }
                }
                s += "</tr></thead><tbody>";
                s += "<tr><td>ĐK</td>";
                for (int i = 0; i < countTTY * 2; i++)
                {
                    s += "<td></td>";
                }
                //decimal[] tong = new decimal[countTTY * 3];
                //int tonIndex = countTTY * 2;
                foreach (DataRow r in tblTTY.Rows)
                {
                    s += "<td>" + Config.ToXVal1(dt.Rows[0][r["IDVatTu"].ToString()]) + "</td>";
                    //tong[tonIndex] = Config.ToDecimal(dt.Rows[0][r["IDVatTu"].ToString()]);
                    //tonIndex++;
                }
                s += "<td></td></tr>";
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];
                    s += "<tr>";
                    s += "<td>" + ((DateTime)r["NgayBienDong"]).ToString("dd/MM/yyyy") + "</td>";
                    //int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        s += "<td>" + Config.ToXVal(r["Nhap" + rTTY["IDVatTu"].ToString()]) + "</td>";
                        //tong[j] += Config.ToDecimal(r["Nhap" + rTTY["IDVatTu"].ToString()]);
                        //j++;
                    }
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        s += "<td>" + Config.ToXVal(r["Xuat" + rTTY["IDVatTu"].ToString()]) + "</td>";
                        //tong[j] += Config.ToDecimal(r["Xuat" + rTTY["IDVatTu"].ToString()]);
                        //j++;
                    }
                    //int j = countTTY * 2;
                    int j = 0;
                    foreach (DataRow rTTY in tblTTY.Rows)
                    {
                        if (r[rTTY["IDVatTu"].ToString()] == DBNull.Value)
                        {
                            s += "<td>" + Config.ToXVal1(tong1[j]) + "</td>";
                        }
                        else
                        {
                            s += "<td>" + Config.ToXVal1(r[rTTY["IDVatTu"].ToString()]) + "</td>";
                            tong1[j] = Config.ToDecimal(r[rTTY["IDVatTu"].ToString()]);
                        }
                        j++;
                    }
                    s += "<td></td></tr>";
                }

                //Tính tổng
                s += "<tr style='font-weight:bold; vertical-align:middle;'><td>T.C</td>";
                for (int i = 0; i < countTTY * 3; i++)
                {
                    s += "<td>" + Config.ToXVal1(tong[i]) + "</td>";
                }
                s += "</tr>";
                s += "</tbody></table>";
                lt.Text = s;
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}