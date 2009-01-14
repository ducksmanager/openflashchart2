﻿package elements {

	import flash.display.Sprite;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	
	 import flash.text.TextField;
    import flash.text.TextFieldType;
	import flash.text.TextFormat;

	public class CameraIcon extends Sprite {
		private var chartId:String;

		public function CameraIcon(chartId:String) {
			
			this.buttonMode = true;
			this.useHandCursor = true;
			this.chartId = chartId;
			
			
			this.alpha = 0.5;
			
			this.graphics.beginFill(0xffffff, .8);
			this.graphics.drawRoundRect(0, 0, 80, 20, 5, 5 );
			this.graphics.endFill();
			
			this.add_text();
			this.draw_camera();
			
			this.addEventListener(MouseEvent.CLICK, mouseClickHandler);
			this.addEventListener(MouseEvent.MOUSE_DOWN, mouseDownHandler);
			this.addEventListener(MouseEvent.MOUSE_OVER, mouseOverHandler);
			this.addEventListener(MouseEvent.MOUSE_OUT, mouseOutHandler);
		}
		
		private function draw_camera():void {
			
			this.graphics.beginFill(0x000000, .2);
			this.graphics.drawRoundRect(2, 4, 26, 15, 2, 2);
			this.graphics.drawRect(20, 1, 5, 3);
			this.graphics.endFill();

			this.graphics.beginFill(0x000000, .3);
			this.graphics.drawCircle(8, 11, 5);
			this.graphics.endFill();
			
		}
		
		private function add_text(): void {
			var title:TextField = new TextField();
            title.x = 35;
			title.y = 0;
			
			//this.text = 'Save chart';
			
			title.htmlText = 'Save chart';
			
			var fmt:TextFormat = new TextFormat();
			fmt.color = 0;// this.css.color;
//			fmt.font = this.css.font_family?this.css.font_family:'Verdana';
//			fmt.bold = this.css.font_weight == 'bold'?true:false;
			fmt.size = 10;// this.css.font_size;
//			fmt.align = "center";
		
			title.setTextFormat(fmt);
			title.autoSize = "left";
			
//			title.border = true;
			

			this.addChild(title);
		}

		public function mouseClickHandler(event:MouseEvent):void {
			this.alpha = 0.0;
			tr.ace('Save Image');
			ExternalInterface.call("save_image", this.chartId);
			this.alpha = 1.0;
		}

		public function mouseOverHandler(event:MouseEvent):void {
			this.alpha = 0.9;
		}
		
		public function mouseDownHandler(event:MouseEvent):void {
			this.alpha = 1.0;
		}

		public function mouseOutHandler(event:MouseEvent):void {
			this.alpha = 0.5;
		}
	}
}