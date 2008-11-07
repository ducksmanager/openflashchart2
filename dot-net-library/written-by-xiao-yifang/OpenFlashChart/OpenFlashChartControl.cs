using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using System.Web.UI;

namespace OpenFlashChart
{
    [Designer(typeof(ChartControlDesigner)), Description("Chart control for open flash chart"), ToolboxData("<{0}:OpenFlashChartControl runat=\"server\" ></{0}:OpenFlashChartControl>")]
    public class OpenFlashChartControl : Control
    {
        private int width;
        private int height;
        private string externalSWFfile;
        private string externalSWFObjectFile;
        private string loadingmsg;
        private OpenFlashChart chart;
        private bool _enableCache;
        /// <summary>
        /// Used to hold internal chart
        /// </summary>
        public OpenFlashChart Chart
        {
            get{ return chart;}
            set{ chart = value;}
        }
       
        private string datafile;

        [DefaultValue(600)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int Width
        {
            get
            {
                width = 600;
                if (this.ViewState["width"] != null)
                {
                    width = Convert.ToInt32(this.ViewState["width"]);
                }
                return width;
            }
            set
            {
                this.ViewState["width"] = value;
                width = value;
            }
        }
        [DefaultValue(300)]
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int Height
        {
            get
            {
                height = 300;
                if (this.ViewState["height"] != null)
                {
                    height = Convert.ToInt32(this.ViewState["height"]);
                }
                return height;
            }
            set
            {
                this.ViewState["height"] = value;
                height = value;
            }
        }
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ExternalSWFfile
        {
            get { return externalSWFfile; }
            set { externalSWFfile = value.Trim(); }
        }
        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ExternalSWFObjectFile
        {
            get { return externalSWFObjectFile; }
            set { externalSWFObjectFile = value.Trim(); }
        }
        
       
        public string DataFile
        {
            get { return datafile; }
            set { datafile = value; }
        }

        public string LoadingMsg
        {
            get { return loadingmsg; }
            set { loadingmsg = value; }
        }

        public bool EnableCache
        {
            get { return _enableCache; }
            set { _enableCache = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            const string key = "swfobject";
            string swfobjectfile = ExternalSWFObjectFile;
            if (string.IsNullOrEmpty(ExternalSWFObjectFile))
                swfobjectfile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "OpenFlashChart.swfobject.js");
            
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(key))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), key, "<script type=\"text/javascript\" src=\"" + swfobjectfile + "\"></script>");
            }
            base.OnInit(e);
        }
        public override void RenderControl(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(ExternalSWFfile))
                ExternalSWFfile = Page.ClientScript.GetWebResourceUrl(this.GetType(), "OpenFlashChart.open-flash-chart.swf");
            builder.AppendFormat("<div id=\"{0}\">", this.ClientID);
            builder.AppendLine("</div>");
            builder.AppendLine("<script type=\"text/javascript\">");
            builder.AppendFormat("swfobject.embedSWF(\"{0}\", \"{1}\", \"{2}\", \"{3}\",\"9.0.0\", \"expressInstall.swf\",",
                ExternalSWFfile, this.ClientID, Width, Height);
            builder.Append("{\"data-file\":\"");
            //if both chart,datafile exists ,chart win.
            if(chart!=null)
            {
                if (!EnableCache)
                    Page.Cache.Remove(this.ClientID);
                Page.Cache.Add(this.ClientID, chart.ToString(), null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0),
                              CacheItemPriority.Normal, null);
                builder.Append("ofc_handler.ofc?chartjson=" + this.ClientID + "&ec=" + (EnableCache ? "1" : "0"));
            }
            else
                builder.Append(DataFile);
            builder.Append("\"");
            if (!string.IsNullOrEmpty(loadingmsg))
            {
                builder.AppendFormat(",\"loading\":\"{0}\"", loadingmsg);
            }
            builder.Append("});");
            builder.AppendLine("</script>");
           
            writer.Write(builder.ToString());
            base.RenderControl(writer);
        }
    }
}
