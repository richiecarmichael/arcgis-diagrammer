using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Crainiate.Diagramming
{
	public interface IDiagram
	{
		//Properties
		Elements Shapes {get; set;}
		Elements Lines {get; set;}
		Layers Layers {get; set;}
		
		Color BackColor {get; set;}
		Size DiagramSize {get; set;}

		IRender Render {get;}
	}

	public interface IContainer
	{
		Elements Shapes {get; set;}
		Elements Lines {get; set;}
		Margin Margin {get;set;}
		RenderList RenderList {get;}
		Layouts.Route Route {get; set;}
		PointF Offset {get;}
		SizeF ContainerSize {get;}
		bool CheckBounds {get; set;}
		
		//Methods
		bool Contains(PointF location);
		bool Contains(PointF location,bool transparent);
		bool Contains(PointF location, Margin margin);
	}	

	public interface IRender
	{
		//Properties
		Color BackColor {get; set;}
		CompositingMode CompositingMode  {get; set;}
		CompositingQuality CompositingQuality {get; set;}
		PixelOffsetMode PixelOffsetMode {get; set;}
		RenderList Elements {get; set;}
		Layers Layers {get; set;}
		Rectangle RenderRectangle  {get; set;}
		SizeF DiagramSize {get; set;}
		float Zoom {get; set;}
		float ZoomFactor {get;}
		float ScaleFactor {get;}
		Layer CurrentLayer {get;}
		Bitmap Bitmap {get;}
		System.Drawing.Image BackgroundImage {get; set;}

		//Methods
		Color AdjustColor(Color color, float width,float opacity);
		void RenderDiagram(Rectangle renderRectangle);
		void RenderDiagramElements(Graphics graphics);
	}

	public interface IRenderDesign
	{
		//Properties
		Color GridColor {get; set;}
		Size GridSize {get; set;}
		GridStyle GridStyle {get; set;}
		Matrix Transform {get;}
		bool DrawGrid {get; set;}
		bool DrawSelections {get; set;}
		Rectangle SelectionRectangle {get; set;}
		RenderList Actions {get; set;}
		RenderList Highlights {get; set;}
		GraphicsPath DecorationPath {get; set;}
		bool Locked {get;}
		string Feedback {get; set;}
		Point FeedbackLocation {get; set;}
		ActionStyle ActionStyle {get; set;}
		SizeF PageLineSize {get; set;}
		bool DrawPageLines {get; set;}
		int RenderTime {get;}
		RectangleF Vector {get; set;}
		
		//Methods
		void Lock();
		void Unlock();
	}

	public interface IRenderPaged
	{
		Color WorkspaceColor {get; set;}
		Size WorkspaceSize {get; set;}
		bool Paged {get; set;}
		Point PagedOffset {get; set;}
		SizeF PagedSize {get; set;}
	}

	public interface IExpandable
	{
		bool Expanded {get; set;}
		GraphicsPath Expander {get;}
		bool DrawExpand {get; set;}
		SizeF ContractedSize {get; set;}
		SizeF ExpandedSize {get; set;}
	}

	public interface IMouseEvents
	{
		bool OnMouseDown(MouseEventArgs e);
		bool OnMouseMove(MouseEventArgs e);
		bool OnMouseUp(MouseEventArgs e);
	}

	public interface ISelectable
	{
		event EventHandler SelectedChanged;
		bool DrawSelected {get; set;}
		bool Selected {get; set;}
	}

	public interface IUserInteractive
	{
		UserInteraction Interaction {get; set;}
	}

	public interface IPortContainer
	{
		Elements Ports {get; set;}

		void LocatePort(Port port);
		PortOrientation GetPortOrientation(Port port,PointF location);
		bool ValidatePortLocation(Port port,PointF location);
		float GetPortPercentage(Port port,PointF location);
	}

	public interface ILabelContainer
	{
		PointF GetLabelLocation();
		SizeF GetLabelSize();
	}

	public interface ITransformable
	{
		float Rotation {get; set;}
		GraphicsPath TransformPath {get;}
		RectangleF TransformRectangle {get;}
	}

	public interface IAnimatable
	{
		Animation Animation {get; set;}
	}
}

namespace Crainiate.Diagramming.Editing
{
	public interface ILabelEdit
	{
		StringFormat StringFormat {get; set;}
		string Text {get; set;}
		bool Visible {get; set;}
		bool Cancelled {get;}
		float Zoom {get; set;}
		AutoSizeMode AutoSize {get; set;}

		void SendEnd();
		void SendHome();

		event EventHandler Complete;
		event EventHandler Cancel;
	}
}

