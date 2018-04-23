using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using DevExpress.XtraBars;
using System.Drawing;

namespace DXSample
{
    public class RibbonDragDropHelper
    {
        RibbonControl ribbon;
        RibbonHitInfo hitInfo = null;
        BarItemLink link = null;


        public RibbonDragDropHelper(RibbonControl ribbon)
        {
            this.ribbon = ribbon;
        }

        public void EnableQATDragDrop()
        {
            ribbon.AllowDrop = true;
            ribbon.MouseDown += OnMouseDown;
            ribbon.MouseMove += OnMouseMove;
            ribbon.DragOver += OnDragOver;
            ribbon.DragDrop += OnDragDrop;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            RibbonControl ribbonControl = sender as RibbonControl;
            Point point = ribbonControl.PointToClient(new Point(e.X, e.Y));
            RibbonHitInfo hitInfo = ribbonControl.CalcHitInfo(point);
            BarItemLink targetLink = hitInfo.Item;
            if (hitInfo.InToolbar && targetLink != null && !targetLink.Equals(link))
                ReorderItems(ribbonControl, targetLink);
            link = null;
        }

        private void ReorderItems(RibbonControl ribbonControl, BarItemLink targetLink)
        {
            int targetIndex = ribbonControl.Toolbar.ItemLinks.IndexOf(targetLink);
            int destIndex = ribbonControl.Toolbar.ItemLinks.IndexOf(link);
            ribbonControl.Toolbar.ItemLinks.Insert(link, targetLink.Item);
            ribbonControl.Toolbar.ItemLinks.Insert(targetLink, link.Item);
            ribbonControl.Toolbar.ItemLinks.Remove(targetLink);
            ribbonControl.Toolbar.ItemLinks.Remove(link);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            RibbonControl ribbonControl = sender as RibbonControl;
            Point point = ribbonControl.PointToClient(new Point(e.X, e.Y));
            RibbonHitInfo hitInfo = ribbonControl.CalcHitInfo(point);
            e.Effect = DragDropEffects.None;
            if (hitInfo.InToolbar && hitInfo.Item != null)
                e.Effect = DragDropEffects.Move;
        }


        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            RibbonControl ribbonControl = sender as RibbonControl;
            hitInfo = ribbonControl.CalcHitInfo(e.Location);
            if (!hitInfo.InToolbar || hitInfo.Item == null)
                hitInfo = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            RibbonControl ribbonControl = sender as RibbonControl;
            if (e.Button == MouseButtons.Left && hitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(hitInfo.HitPoint.X - dragSize.Width / 2,
                    hitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(e.Location))
                {
                    link = hitInfo.Item;
                    ribbonControl.DoDragDrop(link, DragDropEffects.Move);
                    hitInfo = null;
                }
            }
        }

        public void DisableQATDragDrop()
        {
            ribbon.AllowDrop = false;
            ribbon.MouseDown -= OnMouseDown;
            ribbon.MouseMove -= OnMouseMove;
            ribbon.DragOver -= OnDragOver;
            ribbon.DragDrop -= OnDragDrop;
        }


    }
}
