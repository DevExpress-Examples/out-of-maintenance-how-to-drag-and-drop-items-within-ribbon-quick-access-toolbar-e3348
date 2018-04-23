Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports DevExpress.Skins
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Ribbon.ViewInfo
Imports DevExpress.XtraBars
Imports System.Drawing

Namespace DXSample
	Public Class RibbonDragDropHelper
		Private ribbon As RibbonControl
		Private hitInfo As RibbonHitInfo = Nothing
		Private link As BarItemLink = Nothing


		Public Sub New(ByVal ribbon As RibbonControl)
			Me.ribbon = ribbon
		End Sub

		Public Sub EnableQATDragDrop()
			ribbon.AllowDrop = True
			AddHandler ribbon.MouseDown, AddressOf OnMouseDown
			AddHandler ribbon.MouseMove, AddressOf OnMouseMove
			AddHandler ribbon.DragOver, AddressOf OnDragOver
			AddHandler ribbon.DragDrop, AddressOf OnDragDrop
		End Sub

		Private Sub OnDragDrop(ByVal sender As Object, ByVal e As DragEventArgs)
			Dim ribbonControl As RibbonControl = TryCast(sender, RibbonControl)
			Dim point As Point = ribbonControl.PointToClient(New Point(e.X, e.Y))
			Dim hitInfo As RibbonHitInfo = ribbonControl.CalcHitInfo(point)
			Dim targetLink As BarItemLink = hitInfo.Item
			If hitInfo.InToolbar AndAlso targetLink IsNot Nothing AndAlso (Not targetLink.Equals(link)) Then
				ReorderItems(ribbonControl, targetLink)
			End If
			link = Nothing
		End Sub

		Private Sub ReorderItems(ByVal ribbonControl As RibbonControl, ByVal targetLink As BarItemLink)
			Dim targetIndex As Integer = ribbonControl.Toolbar.ItemLinks.IndexOf(targetLink)
			Dim destIndex As Integer = ribbonControl.Toolbar.ItemLinks.IndexOf(link)
			ribbonControl.Toolbar.ItemLinks.Insert(link, targetLink.Item)
			ribbonControl.Toolbar.ItemLinks.Insert(targetLink, link.Item)
			ribbonControl.Toolbar.ItemLinks.Remove(targetLink)
			ribbonControl.Toolbar.ItemLinks.Remove(link)
		End Sub

		Private Sub OnDragOver(ByVal sender As Object, ByVal e As DragEventArgs)
			Dim ribbonControl As RibbonControl = TryCast(sender, RibbonControl)
			Dim point As Point = ribbonControl.PointToClient(New Point(e.X, e.Y))
			Dim hitInfo As RibbonHitInfo = ribbonControl.CalcHitInfo(point)
			e.Effect = DragDropEffects.None
			If hitInfo.InToolbar AndAlso hitInfo.Item IsNot Nothing AndAlso Not(TypeOf hitInfo.Item Is DevExpress.XtraBars.Ribbon.Internal.RibbonQuickToolbarBarItemLink) Then
				e.Effect = DragDropEffects.Move
			End If
		End Sub


		Private Sub OnMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim ribbonControl As RibbonControl = TryCast(sender, RibbonControl)
			hitInfo = ribbonControl.CalcHitInfo(e.Location)
			If (Not hitInfo.InToolbar) OrElse hitInfo.Item Is Nothing OrElse TypeOf hitInfo.Item Is DevExpress.XtraBars.Ribbon.Internal.RibbonQuickToolbarBarItemLink Then
				hitInfo = Nothing
			End If
		End Sub

		Private Sub OnMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim ribbonControl As RibbonControl = TryCast(sender, RibbonControl)
			If e.Button = MouseButtons.Left AndAlso hitInfo IsNot Nothing Then
				Dim dragSize As Size = SystemInformation.DragSize
				Dim dragRect As New Rectangle(New Point(hitInfo.HitPoint.X - dragSize.Width / 2, hitInfo.HitPoint.Y - dragSize.Height / 2), dragSize)

				If (Not dragRect.Contains(e.Location)) Then
					link = hitInfo.Item
					ribbonControl.DoDragDrop(link, DragDropEffects.Move)
					hitInfo = Nothing
				End If
			End If
		End Sub

		Public Sub DisableQATDragDrop()
			ribbon.AllowDrop = False
			RemoveHandler ribbon.MouseDown, AddressOf OnMouseDown
			RemoveHandler ribbon.MouseMove, AddressOf OnMouseMove
			RemoveHandler ribbon.DragOver, AddressOf OnDragOver
			RemoveHandler ribbon.DragDrop, AddressOf OnDragDrop
		End Sub


	End Class
End Namespace
