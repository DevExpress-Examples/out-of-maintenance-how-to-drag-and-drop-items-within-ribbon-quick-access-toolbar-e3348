Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraBars.Ribbon.ViewInfo
Imports DevExpress.XtraBars


Namespace DXSample
	Partial Public Class Main
		Inherits XtraForm

		Private helper As RibbonDragDropHelper

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub OnFormLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			For Each item As BarItem In ribbonControl1.Items
				ribbonControl1.Toolbar.ItemLinks.Add(item)
			Next item
			helper = New RibbonDragDropHelper(ribbonControl1)
			helper.EnableQATDragDrop()
		End Sub

		Protected Overrides Sub OnFormClosing(ByVal e As FormClosingEventArgs)
			MyBase.OnFormClosing(e)
			helper.DisableQATDragDrop()
		End Sub
	End Class
End Namespace
