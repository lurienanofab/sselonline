(function($){
	$.fn.alerts = function(options){
		return this.each(function(){
			var $this = $(this);
			var opt = $.extend({}, {"url":"", "location": "default", "callback": null}, options);
			
			var displayError = function(msg){
				$this.html("").append($("<div/>").addClass("alerts-error").html(msg));
			}
			
			if (opt.url) {
			    var def = $.Deferred();

				var promise = $.ajax({
				    "url": opt.url + '?ts=' + (new Date()).valueOf(),
					"dataType": "json",
                    "success": function (data, textStatus, jqXHR) {
						$.each(data, function(index, item){
						    item = $.extend({ "location": "default", "type": "info" }, item);
							var sdate = new Date(item.startDate);
							var edate = new Date(item.endDate);
                            var now = new Date();
                            if (item.location == opt.location && now >= sdate && now < edate) {
								var div = $("<div/>").addClass("alerts-item " + item.location + " " + item.type)
									.html(item.text)
									.appendTo($this);
									
								if (item.style)
									div.css(item.style);
                            }
						});

						def.resolve(data);
					},
					"error": function(jqXHR, textStatus, errorThrown){
					    displayError("Could not retrieve alerts: " + errorThrown);
					    def.reject(errorThrown);
					}
				});

				if (typeof opt.callback == "function")
				    opt.callback(def);
			}
			else {
				displayError("Option 'url' not specified.");
			}
		});
	}
}(jQuery));