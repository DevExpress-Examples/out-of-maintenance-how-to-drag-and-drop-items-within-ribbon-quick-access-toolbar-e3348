using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using DevExpress.XtraBars;


namespace DXSample {
    public partial class Main: XtraForm {

        RibbonDragDropHelper helper;

        public Main() {
            InitializeComponent();
        }
       
        private void OnFormLoad(object sender, EventArgs e) {
            foreach(BarItem item in ribbonControl1.Items)
                ribbonControl1.Toolbar.ItemLinks.Add(item);
            helper = new RibbonDragDropHelper(ribbonControl1);
            helper.EnableQATDragDrop();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            helper.DisableQATDragDrop();
        }
    }
}
