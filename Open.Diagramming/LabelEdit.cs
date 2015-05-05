using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Editing
{
	[ToolboxItem(false)]
	public class LabelEdit: System.Windows.Forms.Control, ILabelEdit
	{
		//Property variables
		private StringFormat mStringFormat = StringFormat.GenericDefault;
		private bool mControl;
		private int blinkRate = 500;
		private float mZoom = 100;
		private bool mCancelled;
		private AutoSizeMode mAutoSize;
		private bool mAllowEnter;

		//Working variables
		private System.ComponentModel.Container components = null;
		private RectangleF[] mCharacterRectangles;
		private Bitmap mRenderBitmap = null;
		private Timer mTimer;
		private bool mCaretOn;
		private int mCaret;
		private int mSelect;

		private Stack mUndo = new Stack();
		private Stack mRedo = new Stack();

		private bool mCompleted;

		//Constants
		private const int WM_KEYDOWN = 0x100;
		private const int WM_SYSKEYDOWN = 0x104;

		#region Interface

		[Category("Action"),Description("Occurs when the user wants to complete editing.")]
		public event EventHandler Complete;
		[Category("Action"),Description("Occurs when the user wants to cancel editing.")]
		public event EventHandler Cancel;

		//Constructors
		public LabelEdit():base()
		{
			//Check for expiry
			Component.Instance.GetLicense(typeof(LabelEdit), this);

			InitializeComponent();
			mControl = true;
			mStringFormat = new StringFormat();
			//SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//this.BackColor = Color.Transparent;
			
			this.Cursor = Cursors.IBeam;
		}

		public LabelEdit(bool RenderOnly)
		{
			InitializeComponent();
			mControl = !RenderOnly;
			mStringFormat = new StringFormat();
		}

		//Properties
		[Category("Data"),Description("Sets or gets the text displayed by this control.")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				mUndo.Push(mCaret); //Put the caret position on the undo stack
				mUndo.Push(base.Text); //Put the text on the undo stack
				base.Text = value;

				if (AutoSize != AutoSizeMode.None) CheckSize();

				Refresh();
			}
		}
		
		[Category("Behaviour"),Description("Sets or gets the stringformat object used to draw the text.")]
		public virtual StringFormat StringFormat
		{
			get
			{
				return mStringFormat;
			}
			set
			{
				mStringFormat = value;
				Refresh();
			}
		}

		[Category("Behaviour"),Description("Determines how the control grows with the size of the text.")]
		public virtual AutoSizeMode AutoSize
		{
			get
			{
				return mAutoSize;
			}
			set
			{
				if (value != mAutoSize)
				{
					mAutoSize = value;
					if (mAutoSize != AutoSizeMode.None) CheckSize();
				}
			}
		}

		[Category("Behaviour"),Description("Determines if the label editor inserts carriage returns into the text.")]
		public virtual bool AllowEnter
		{
			get
			{
				return mAllowEnter;
			}
			set
			{
				mAllowEnter = value;
			}
		}

		[Category("Data"),Description("Returns the rendered bitmap.")]
		public virtual Bitmap Bitmap
		{
			get
			{
				return mRenderBitmap;
			}
		}

		[Category("Behaviour"),Description("Sets or gets the zoom percentage for the control.")]
		public virtual float Zoom
		{
			get
			{
				return mZoom;
			}
			set
			{
				if (mZoom != value)
				{
					mZoom = value;
					Refresh();
				}
			}
		}

		[Category("Data"),Description("Sets or gets the start of the text selection.")]
		public virtual int SelectionStart
		{
			get
			{
				return mSelect;
			}
			set
			{
				if (value >= 0 && value <= Text.Length)
				{
					mSelect = value;
					mCaret = value;

					Refresh();
				}
			}
		}

		[Category("Data"),Description("Sets or gets the position of the end of the text selection.")]
		public virtual int SelectionEnd
		{
			get
			{
				return mCaret;
			}
			set
			{
				if (value >= 0 && value <= Text.Length)
				{
					mCaret = value;
					Refresh();
				}
			}
		}

		[Category("Data"),Description("Returns a value indicating whether editing has been cancelled.")]
		public virtual bool Cancelled
		{
			get
			{
				return mCancelled;
			}
		}

		//Methods
		public virtual void Undo()
		{
			if (mUndo.Count > 0)
			{
				mRedo.Push(mCaret); //Add the caret postion to the redo
				mRedo.Push(Text); //Add the text to the redo
				base.Text = (string) mUndo.Pop();
				mCaret = (int) mUndo.Pop();
				mSelect = mCaret;
				Refresh();
			}
		}

		public virtual void Redo()
		{
			if (mRedo.Count > 0)
			{
				Text = (string) mRedo.Pop();
				mCaret = (int) mRedo.Pop();
				mSelect = mCaret;
			}
		}

		[Description("Renders the control onto the supplied graphics handle")]
		public virtual void Render(Graphics graphics)
		{
			RenderLabel(graphics);
		}

		[Description("Returns an array of rectangles containing the measurements of the control's text")]
		public virtual RectangleF[] MeasureCharacters()
		{
			MeasureCharactersImplementation(Graphics.FromImage(mRenderBitmap));
			return mCharacterRectangles;
		}

		[Description("Returns an array of rectangles containing the measurements of the control's text")]
		public virtual RectangleF[] MeasureCharacters(Graphics graphics)
		{
			MeasureCharactersImplementation(graphics);
			return mCharacterRectangles;
		}

		[Description("Returns a rectangle containing the measurement of the control's text")]
		public virtual SizeF MeasureText()
		{
			return MeasureTextImplementation(Graphics.FromImage(mRenderBitmap));
		}

		[Description("Returns a rectangle containing the measurement of the control's text")]
		public virtual SizeF MeasureText(Graphics graphics)
		{
			return MeasureTextImplementation(graphics);
		}

		[Description("Sends a character to the current caret position in the annotation text.")]
		public virtual void SendCharacter(char Value)
		{
			InsertCharacter(Value);
		}

		[Description("Sends a delete character to the current caret position in the annotation text.")]
		public virtual void SendDelete()
		{
			DeleteCharacter();
		}

		[Description("Sends a backspace character to the current caret position in the annotation text.")]
		public virtual void SendBackSpace()
		{
			BackspaceCharacter();
		}

		[Description("Sends an arrow character to the annotation.")]
		public virtual void SendArrow(Keys Key)
		{
			ArrowCharacter(Key);
		}

		[Description("Sends an end keystroke to the annotation.")]
		public virtual void SendEnd()
		{
			EndCharacter();
		}

		[Description("Sends a home keystroke to the annotation.")]
		public virtual void SendHome()
		{
			HomeCharacter();
		}

		//Raises the Complete event
		protected virtual void OnComplete()
		{
			if (!mCompleted)
			{
				mCompleted = true;
				if (Complete != null) Complete(this,EventArgs.Empty);
			}
		}

		//Raises the Cancel event
		protected virtual void OnCancel()
		{
			if (Cancel != null) Cancel(this,EventArgs.Empty);
		}

		#endregion

		#region Component Designer generated code

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Overrides

		public override void Refresh()
		{
			if (mControl) 
			{
				UpdateBuffer();
				base.Refresh();
			}
		}

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				Refresh();
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				Refresh();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Focused) 
			{
				if (mTimer != null) mTimer.Stop();
				mCaretOn = true;
				BlinkCaret();

				mSelect = CaretFromLocation(e.X,e.Y);
				mCaret = mSelect;
				Refresh();
			}
			base.OnMouseDown (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Focused && e.Button == MouseButtons.Left) 
			{
				mCaret = CaretFromLocation(e.X,e.Y);
				Refresh();
			}
			base.OnMouseMove (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Focused) 
			{
				BlinkCaret();
				if (mTimer != null) mTimer.Start();
			}
			base.OnMouseUp(e);
		}

		//Turn on caret
		protected override void OnGotFocus(EventArgs e)
		{
			mCaretOn = false;

			mTimer = new Timer();
			mTimer.Interval = blinkRate;
			mTimer.Tick +=new EventHandler(Timer_Tick);

			SendEnd();
			mSelect = mCaret;
			UpdateCaret();

			BlinkCaret();
			mTimer.Start();
			
			base.OnGotFocus(e);
		}

		//Turn off caret
		protected override void OnLostFocus(EventArgs e)
		{
			if (mTimer != null) mTimer.Stop();
			mCaretOn = true;
			BlinkCaret();
			
			OnComplete();

			mTimer = null;

			base.OnLostFocus(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.DrawImageUnscaled(mRenderBitmap,new Point(0,0));	
			base.OnPaint(pe);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground (pevent);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);

			if (DisplayRectangle.Width == 0 || DisplayRectangle.Height == 0) return;
			mRenderBitmap = new Bitmap(DisplayRectangle.Width,DisplayRectangle.Height,PixelFormat.Format32bppPArgb);
			RenderLabel(Graphics.FromImage(mRenderBitmap));
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			//Stop displaying caret
			if (mTimer != null) mTimer.Stop();
			mCaretOn = false;
			BlinkCaret();
			
			SendCharacter(e.KeyChar);
			UpdateCaret(); //Will restart timer

			e.Handled = true;

			base.OnKeyPress(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
			{
				//Shift left
				if (keyData == (Keys.Shift | Keys.Left))
				{
					SendArrow(Keys.Left);
					Refresh();
					UpdateCaret();
					return true;
				}
				
				//Shift right
				if (keyData == (Keys.Shift | Keys.Right))
				{
					SendArrow(Keys.Right);
					Refresh();
					UpdateCaret();
					return true;
				}

				//Shift Home
				if (keyData == (Keys.Shift | Keys.Home))
				{
					SendHome();
					Refresh();
					UpdateCaret();
					return true;
				}

				//Shift End
				if (keyData == (Keys.Shift | Keys.End))
				{
					SendEnd();
					Refresh();
					UpdateCaret();
					return true;
				}

				//Undo
				if (keyData == (Keys.Control | Keys.Z))
				{
					Undo();
					return true;
				}

				//Redo
				if (keyData == (Keys.Control | Keys.Y))
				{
					Redo();
					return true;
				}
				
				//Enter
				if (keyData == Keys.Enter)
				{
					//Stop displaying caret
					if (mTimer != null) mTimer.Stop();
					mCaretOn = false;
					BlinkCaret();

					if (AllowEnter)
					{
						SendCharacter(Convert.ToChar("\n"));
						mSelect = mCaret;
						Refresh();
						UpdateCaret();
					}
					else
					{
						OnComplete();
					}

					return true;
				}

				//Left
				if (keyData == Keys.Left)
				{
					SendArrow(Keys.Left);
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Right
				if (keyData == Keys.Right)
				{
					SendArrow(Keys.Right);
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Top
				if (keyData == Keys.Up)
				{
					SendArrow(Keys.Up);
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Bottom
				if (keyData == Keys.Down)
				{
					SendArrow(Keys.Down);
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Backspace
				if (keyData == Keys.Back)
				{
					SendBackSpace();
					UpdateCaret();
					return true;
				}

				//Delete
				if (keyData == Keys.Delete)
				{
					SendDelete();
					UpdateCaret();
					return true;
				}

				//Home
				if (keyData == Keys.Home)
				{
					SendHome();
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//End
				if (keyData == Keys.End)
				{
					SendEnd();
					mSelect = mCaret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Escape
				if (keyData == Keys.Escape)
				{
					mCancelled = true;
					OnCancel();
					return true;
				}
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Events

		private void Timer_Tick(object sender, EventArgs e)
		{
			BlinkCaret();
		}

		#endregion

		#region Implementation
		
		private void UpdateBuffer()
		{
			if (mRenderBitmap == null) return;
			Graphics graphics = Graphics.FromImage(mRenderBitmap);
			graphics.Clear(BackColor);
			RenderLabel(graphics);
			graphics.Dispose();
		}
		
		private void RenderLabel(Graphics graphics)
		{
			float scale = mZoom / 100;
			RectangleF scaleRect = DisplayRectangle;

			//Apply local transformation
			if (mZoom != 100)
			{
				scaleRect = new RectangleF(0,0,DisplayRectangle.Width / scale,DisplayRectangle.Height / scale);
				graphics.ScaleTransform(scale,scale);
			}

			string text = Text;
			graphics.DrawString(text,Font,new SolidBrush(ForeColor),scaleRect,mStringFormat);
						
			MeasureCharactersImplementation(graphics);

			//Draw selection if applicable
			//Reset the scale because thats how the characters have been measured
			graphics.ScaleTransform(1/scale,1/scale);
			DrawSelection(graphics);
		}

		//make sure caret is re-rendered
		private void UpdateCaret()
		{
			if (this.Focused)
			{
				if (mTimer != null) mTimer.Stop();
				mCaretOn = true;
				BlinkCaret();
				BlinkCaret();
				if (mTimer != null) mTimer.Start();
			}
		}
		
		private void BlinkCaret()
		{
			Graphics graphics = CreateGraphics();
			
			try
			{
				//Invert whatever the last action was
				if (mCaretOn)
				{
					graphics.DrawImageUnscaled(mRenderBitmap, new Point(0,0));
				}
				else
				{
					float scale = mZoom / 100;
					float caretWidth = 1 / scale;
					graphics.ScaleTransform(scale,scale);
			
					//Draw caret as black rectangle
					SolidBrush brush = new SolidBrush(Color.Black);
					RectangleF rect = mCharacterRectangles[mCaret];
					graphics.FillRectangle(brush,rect.X+rect.Width,rect.Y,caretWidth,rect.Height);
				}
				mCaretOn = ! mCaretOn;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error blinking caret " + ex.ToString());	
			}
			graphics.Dispose();
		}

		private void DrawSelection(Graphics graphics)
		{
			if (mSelect != mCaret)
			{
				SolidBrush brush = new SolidBrush(Color.FromArgb(64,SystemColors.Highlight));

				int start;
				int end;

				//Calculate start and end
				if (mSelect < mCaret)
				{
					start = mSelect+1;
					end = mCaret;
				}
				else
				{
					start = mCaret+1;
					end = mSelect;
				}

				//Render each rectangle
				for (int i = start; i<= end;i++)
				{
					graphics.FillRectangle(brush,mCharacterRectangles[i]);
				}
			}
		}
		
		private int CaretFromLocation(int x, int y)
		{
			if (Text == null || Text == "") return 0;

			//Scale the points
			float scale = mZoom / 100;
			float cx = x / scale;
			float cy = y / scale;
			int caret;
						
			try
			{
				//Reset the caret
				caret = -1;

				//Check if the point is anywhere in one of the character rectangles
				if (mCharacterRectangles != null)
				{
					for (int i = 0; i <= mCharacterRectangles.GetUpperBound(0); i++)
					{
						if (mCharacterRectangles[i].Contains(cx, cy)) caret = i;
					}
				}

				//If we havent hit a rectangle, then caret must be at the beginning or the end
				if (caret == -1)
				{
					if (cy <= mCharacterRectangles[0].Y)
					{
						caret = 0;
					}
					else
					{
						caret = mCharacterRectangles.GetUpperBound(0);
					}
				}

			}
			catch
			{
				caret = 0;
			}
			return caret;
		}

		private void InsertCharacter(char Value)
		{
			string restore = Text;

			try
			{
				//Clear any selections
				if (mSelect != mCaret) DeleteSelection();
				
				string text = "";

				if (mCaret > 0 && Text.Length >= mCaret) text = Text.Substring(0, mCaret);
				text += Value.ToString();
				if (mCaret < Text.Length) text += Text.Substring(mCaret);
				Text = text;

				//Move the caret one along
				mCaret += 1;
				mSelect += 1;
			}
			catch (Exception ex)
			{
				Text = restore;
			}
		}

		private void DeleteCharacter()
		{
			//Perform selection delete instead)
			if (mSelect != mCaret)
			{
				DeleteSelection();
				return;
			}

			string restore = Text;

			try
			{
				string text = "";

				if (mCaret > 0)	text = Text.Substring(0, mCaret);
				if (mCaret < Text.Length) text += Text.Substring(mCaret + 1);

				Text = text;
			}
			catch
			{
				Text = restore;
			}
		}

		private void BackspaceCharacter()
		{
			//Perform selection delete instead)
			if (mSelect != mCaret)
			{
				DeleteSelection();
				return;
			}
			//If at start of line then cant backspace
			if (mCaret == 0) return;
			
			string restore = Text;

			try
			{
				//Reform text
				string text = "";

				text = Text.Substring(0, mCaret - 1);
			
				if (mCaret < Text.Length) text += Text.Substring(mCaret);
				Text = text;

				//Move the caret back one
				mCaret -= 1;
				mSelect = mCaret;
			}
			catch
			{
				Text = restore;
			}
		}

		private void DeleteSelection()
		{
			string text = "";
			
			int start;
			int end;

			//Calculate start and end
			if (mSelect < mCaret)
			{
				start = mSelect;
				end = mCaret-1;
				mCaret -= end-start+1;
			}
			else
			{
				start = mCaret;
				end = mSelect-1;
			}

			string restore = Text;

			try
			{
				if (start > 0) text = Text.Substring(0, start);
				if (end < Text.Length) text += Text.Substring(end + 1);

				mSelect = mCaret;
				Text = text;
			}
			catch
			{
				Text = restore;
			}
		}

		//Process an arrow charater
		private void ArrowCharacter(Keys key)
		{
			if (key == Keys.Left)
			{
				if (mCaret > 0)	mCaret -= 1;
			}
			else if (key == Keys.Right)
			{
				if (mCaret < Text.Length) mCaret += 1;
			}
			else if (key == Keys.Down)
			{
				for (int i = mCaret + 1; i <= Text.Length - 1; i++)
				{
					if (mCharacterRectangles[i].Contains(mCharacterRectangles[mCaret].X, mCharacterRectangles[i].Y))
					{
						mCaret = i;
						break;
					}
				}

			}
			else if (key == Keys.Up)
			{
				for (int i = mCaret - 1; i >= 0; i--)
				{
					if (mCharacterRectangles[i].Contains(mCharacterRectangles[mCaret].X, mCharacterRectangles[i].Y))
					{
						mCaret = i;
						break;
					}
				}
			}
		}

		private void EndCharacter()
		{
			mCaret = Text.Length;
		}

		private void HomeCharacter()
		{
			mCaret = 0;
		}

		//Measures the characters provided into the control character ranges 
		private void MeasureCharactersImplementation(Graphics graphics)
		{
			//Set the character rectangles defined by the character ranges 
			string text = Text;
			Region[] regions = null;
			CharacterRange[] ranges = new CharacterRange[1];
			StringFormat format = (StringFormat) StringFormat.Clone();
			RectangleF scaleRect = DisplayRectangle;
			float scale = mZoom / 100;

			if (mZoom != 100)
			{
				scaleRect = new RectangleF(0,0,DisplayRectangle.Width / scale,DisplayRectangle.Height / scale);
				graphics.ScaleTransform(scale,scale);
			}
			
			//Initialise
			if (text == "") text = ".";
			mCharacterRectangles = new RectangleF[text.Length+1];
			RectangleF rect;

			//Measure into rectangles starting at position 1 in the array
			try
			{
				for (int i = 0; i <= text.Length - 1; i++)
				{
					ranges[0].First = i;
					ranges[0].Length = 1;
					format.SetMeasurableCharacterRanges(ranges);
					regions = graphics.MeasureCharacterRanges(text, Font, scaleRect, format);
					rect = regions[0].GetBounds(graphics);
					mCharacterRectangles[i + 1] = rect;
				}

				mCharacterRectangles[0] = mCharacterRectangles[1];
				mCharacterRectangles[0].Width = 1;
				mCharacterRectangles[0].X -= 1;
			}
			catch (Exception objEx)
			{
				System.Diagnostics.Debug.WriteLine("Error measuring characters" + objEx.ToString());
			}
		}

		private SizeF MeasureTextImplementation(Graphics graphics)
		{
			return graphics.MeasureString(Text, Font, Width, StringFormat);
		}

		//Check to see if width of control smaller than characters
		private void CheckSize()
		{
			Graphics graphics = Graphics.FromImage(mRenderBitmap);

			string text = Text;

			Size size = Size.Round(graphics.MeasureString(text, Font, Width, StringFormat));
			size.Width += 2;
			size.Height += 2;
			
			if (size.Width > Width && (AutoSize == AutoSizeMode.Horizontal || AutoSize == AutoSizeMode.Both)) Width = size.Width;
			if (size.Height > Height && (AutoSize == AutoSizeMode.Vertical || AutoSize == AutoSizeMode.Both)) Height = size.Height;

			Refresh();
		}

		#endregion
	}
}
