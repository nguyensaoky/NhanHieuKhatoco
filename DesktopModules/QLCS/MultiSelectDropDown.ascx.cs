using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Drawing;

namespace DotNetNuke.Modules.QLCS
{
	public partial class MultiSelectDropDown : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.CheckBoxList checkBoxListMain;
		private object dataSource;
		private string dataTextField;
		private string dataValueField;
		private bool autoPostBack;
		private System.Web.UI.Page callingPage;
		public string dataItem;
		public event MultiSelectDropDownDelegate OnItemsSelected;
		public string controlClientID;
		public string lSelect;
        public string type;
        public string typev;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
   				this.hapb.Value = this.AutoPostBack.ToString();
				this.tbm.Attributes.Add("onclick","SHMulSel(" + ControlClientID + ", event)");
                if(typev == null)
                    this.divMain.Attributes["class"] = "DVMain_" + type;
                else
                    this.divMain.Attributes["class"] = "DVMain_" + typev;
                this.dvTop.Attributes["class"] = "DT_" + type;
                this.tbm.Attributes["class"] = "MSTBM_" + type;
			}
		
			if(this.hapb.Value == "True")
			{
				//Register __doPostBack method	
				string key = "funcDoPostBack";
				string script = "<script language=javascript type='text/javascript'>function __doPostBack1(eventTarget, eventArgument) { " + 
					"var theform; " + 
					"if (window.navigator.appName.toLowerCase().indexOf('netscape') > -1) " +
					"{	theform = document.forms['Form1']; 	} " +
					"else "+ 
					"{ theform = document.Form1; } " +
					"theform." + this.ClientID + "_" + "__ET1.value = eventTarget.split('$').join(':'); " +
					"theform." + this.ClientID + "_" + "__EA1.value = eventArgument; " +
					"theform.submit(); }</script>";
                if (CallingPage != null)
				    CallingPage.RegisterClientScriptBlock(key, script);
                else
                    this.Page.RegisterClientScriptBlock(key, script);
			}

			//Handle __doPostBack post back event.
			if(this.hapb.Value == "True" && __ET1 != null && __EA1 != null)// && __EA1.Value != string.Empty)
			{
				if(__ET1.Value.Equals("MultiSelectDropDown"))
				{
					if(this.OnItemsSelected == null)
						return;
					MultiSelectDropDownItemSelectedEventArgs args = new MultiSelectDropDownItemSelectedEventArgs();
					args.SelectedOptionText = __EA1.Value;
					this.tbm.Text = __EA1.Value;
					args.SelectedOptionValueText = hsiv.Value;
					args.SelectedOptionList = GetSelectedOptionListFromText(args.SelectedOptionText);
					this.OnItemsSelected(this, args);
				}
			}
		}	


		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			ControlClientID = "'" + this.ClientID + "'";
			base.OnInit(e);			
		}


		protected override void OnDataBinding(EventArgs e)
		{
			this.dataItem = "DataItem." + this.DataValueField;
			base.OnDataBinding (e);			
		}


		private void InitializeComponent()
		{
			this.rp1.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rp1_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}


		#endregion

		#region Public Methods

		public override void DataBind()
		{
			base.DataBind ();
			this.LoadDataSource();
		}

		public void Clear()
		{
			this.tbm.Text = "";
			this.hsiv.Value = string.Empty;
		}


		#endregion 

		#region Public Properties
		
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				this.dataSource = value;
			}
		}


		public string DataTextField
		{
			get
			{
				return this.dataTextField;			
			}
			set
			{
				this.dataTextField = value;
			}
		}

		public string DataValueField
		{
			get
			{
				return this.dataValueField;
			}
			set
			{
				this.dataValueField = value;
			}
		}


		public string SelectedOptionsText
		{
			get
			{
				return this.tbm.Text;
			}
		}

		public string[] SelectedOptionsList
		{
			get
			{
				string[] optionsList = null, optionListToSend = null;
				if(tbm.Text != string.Empty )
				{
					optionsList = this.tbm.Text.Split(new char[]{','});
					optionListToSend = new string[optionsList.Length - 1];
					for(int index = 0; index < optionsList.Length - 1; index++)
						optionListToSend[index] = optionsList[index].Trim();
				}
				return optionListToSend;
			}
		}

		public string SelectedOptionsValueList
		{
			get
			{
				return this.hsiv.Value;
			}
            set
            {
                this.tbm.Text = "";
                this.hsiv.Value = value;
                string s = " " + value;
                foreach(DataRow r in ((DataTable)DataSource).Rows)
                {
                    if (s.Contains(" " + r[this.dataValueField].ToString() + ","))
                        this.tbm.Text += r[this.dataTextField].ToString() + ", ";
                }
                rp1.DataBind();
            }
		}


		public override bool EnableViewState
		{
			get
			{
				return base.EnableViewState;
			}
			set
			{
				this.EnableViewState = value;
				this.tbm.EnableViewState = value;
				this.rp1.EnableViewState = value;
			}
		}


		public bool AutoPostBack
		{
			get
			{
				return autoPostBack;
			}
			set
			{
				this.autoPostBack = value;
			}
		}

		public System.Web.UI.Page CallingPage
		{
			get
			{
				return this.callingPage;		
			}
			set
			{
				this.callingPage = value;
			}
		}

		public string ControlClientID
		{
			get
			{
				return this.controlClientID;
			}
			set
			{
				this.controlClientID = value;
			}
		}


		#endregion

		#region Private Methods

		private void LoadDataSource()
		{
			if(dataSource == null || dataTextField == null)
				return;
			this.rp1.DataSource = dataSource;
			this.rp1.DataBind();
		}	


		#endregion

		#region Public Events Handlers

		private string[] GetSelectedOptionListFromText(string optionText)
		{
			string[] optionsList = null, optionListToSend = null;
			if(optionText != string.Empty )
			{
				optionsList = optionText.Split(new char[]{','});
				optionListToSend = new string[optionsList.Length - 1];
				for(int index = 0; index < optionsList.Length - 1; index++)
					optionListToSend[index] = optionsList[index].Trim();
			}
			return optionListToSend;
		}

		
		private void rp1_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			HtmlInputCheckBox ctlCheckBox;
			Literal ctlLiteral;

			if (e.Item.ItemType == ListItemType.Header)
				return;
			if (e.Item.ItemIndex != -1)
			{
				ctlCheckBox = e.Item.FindControl("cb1") as HtmlInputCheckBox;
				ctlLiteral = e.Item.FindControl("lt1") as Literal;
				
				if (ctlCheckBox != null && ctlLiteral != null)
				{
                    string s = " " + SelectedOptionsValueList;
                    if (s.Contains(" " + ((DataRowView)e.Item.DataItem).Row[this.dataValueField].ToString() + ",")) ctlCheckBox.Checked = true;
					ctlCheckBox.Attributes.Add("onclick", "SCIT(this, " + ControlClientID + ")");
					ctlLiteral.Text = "<label style='TEXT-TRANSFORM: none; FONT-FAMILY: Arial; FONT-SIZE: 12px; FONT-VARIANT: normal' id='lbl" + e.Item.ItemIndex + "' for='" + ctlCheckBox.ClientID + "'>" + ((DataRowView)e.Item.DataItem).Row[this.dataTextField].ToString() + "</label>";
				}
			}
		}


		#endregion	
	}

	public delegate void MultiSelectDropDownDelegate(object sender, MultiSelectDropDownItemSelectedEventArgs args);

	public class MultiSelectDropDownItemSelectedEventArgs
	{
		private string selectedOptionText;
		private string[] selectedOptionList;
		private string selectedOptionValueText;
		
		public string SelectedOptionText
		{
			get
			{
				return selectedOptionText;
			}
			set
			{
				this.selectedOptionText = value;
			}
		}

		public string SelectedOptionValueText
		{
			get
			{
				return selectedOptionValueText;
			}
			set
			{
				this.selectedOptionValueText = value;
			}
		}

		public string[] SelectedOptionList
		{
			get
			{
				return selectedOptionList;
			}
			set
			{
				this.selectedOptionList = value;
			}
		}		
	}
}