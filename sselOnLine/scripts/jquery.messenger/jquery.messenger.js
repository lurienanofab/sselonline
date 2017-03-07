(function ($) {
    $.fn.messenger = function () {
        return this.each(function () {
            var $this = $(this);

            var currentMessage = 0;

            $(".tabs", $this).tabs({
                activate: function (e, ui) {
                    $(".selected-tab", $this).val(ui.newTab.index());
                }
            });

            var getSelectedTab = function () {
                var result = parseInt($(".selected-tab", $this).val());
                return (isNaN(result)) ? 0 : result;
            }

            var getClientId = function () {
                var result = parseInt($(".client-id", $this).val());
                return (isNaN(result)) ? 0 : result;
            }

            var getSearchOptions = function () {
                var result = { SearchBy: null, Value: null, Subject: null, Body: null, Exclusive: null };
                switch ($(".selected-tab", $this).val()) {
                    case 1:
                        result.SearchBy = "community"
                        result.Value = 0;
                        $(".communities-list input[type=checkbox]:checked", $this).each(function () {
                            result.Value += parseInt($(this).val());
                        });
                        break;
                    case 2:
                        result.SearchBy = "manager";
                        result.Value = $(".managers-list option:selected", $this).val();
                        break;
                    case 3:
                        result.SearchBy = "tools";
                        result.Value = "";
                        var comma = "";
                        $(".tools-list option:selected", $this).each(function () {
                            result.Value += comma + $(this).val();
                            comma = ",";
                        });
                        break;
                    case 4:
                        result.SearchBy = "lab";
                        result.Value = "";
                        var comma = "";
                        $(".areas-list input[type=checkbox]:checked", $this).each(function () {
                            result.Value += comma + $(this).val();
                            comma = ",";
                        });
                        break;
                    default:
                        result.SearchBy = "priv";
                        result.Value = 0;
                        $(".privs-list input[type=checkbox]:checked", $this).each(function () {
                            result.Value += parseInt($(this).val());
                        });
                        break;
                }

                result.Subject = $(".message-subject", $this).val();
                result.Body = $(".message-body", $this).val();
                result.AcknowledgeRequired = $(".message-acknowledge-required").prop("checked");
                result.Exclusive = $(".message-exclusive", $this).prop("checked");

                return result;
            }

            var getRecipients = function (success) {
                var opts = getSearchOptions();
                var url = "/api/messenger/recipients/" + opts.SearchBy + "?value=" + opts.Value;
                $.ajax({ "url": url, "type": "GET", "success": success });
            }

            var getMessages = function (success) {
                var url = "/api/messenger/" + getClientId();
                $.ajax({ "url": url, "type": "GET", "success": success });
            }

            var sendMessage = function (success, error) {
                var opts = getSearchOptions();
                var errors = 0;

                $(".subject-validation", $this).html("");
                $(".body-validation", $this).html("");

                if (!opts.Subject) {
                    $(".subject-validation", $this).append($("<div/>").addClass("validation-error").html("Subject is required"));
                    errors++;
                }

                if (!opts.Body) {
                    $(".body-validation", $this).append($("<div/>").addClass("validation-error").html("Body is required"));
                    errors++;
                }

                if (errors > 0) {
                    error({ "type": "validation" });
                    return;
                }

                var url = "/api/messenger/send";
                $.ajax({ "url": url, "type": "POST", "data": opts, "success": success });
            }

            var viewFirst = function () {
                currentMessage = 0;
                $(".message-item", $this).hide();
                $(".message-item", $this).eq(currentMessage).show();
            }

            var viewNext = function () {
                currentMessage++;
                if (currentMessage == $(".message-item", $this).length)
                    currentMessage = 0;
                $(".message-item", $this).hide();
                $(".message-item", $this).eq(currentMessage).show();
            }

            var getMessageControls = function (item) {
                var result = $("<div/>");
                if (item.AcknowledgeRequired) {
                    result.append($("<input/>").attr("type", "button").attr("value", "Agree").on('click', function (e) {
                        $.ajax({
                            url: "/api/messenger/acknowledge",
                            data: { RecipientID: item.RecipientID },
                            type: "PATCH",
                            success: function (data, textStatus, jqXHR) {
                                if (data == "ok") {
                                    $(".message-item", $this).eq(currentMessage).remove();
                                    if ($(".message-item", $this).length == 0)
                                        $(".message-list").hide()
                                    else {
                                        $(".message-item", $this).hide();
                                        $(".message-item", $this).eq(currentMessage).show();
                                    }
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                var err = $.parseJSON(jqXHR.responseText);
                                $(".control-message", $this).html("").append($("<div/>").addClass("error").html(err.ExceptionMessage))
                            }
                        })
                    }));
                }
                return result;
            }

            getMessages(function (data) {
                if (data.length > 0) {
                    var list = $(".message-list", $this);
                    list.html("");
                    $.each(data, function (index, item) {
                        list.append(
                            $("<li/>").addClass("message-item").css({ "display": "none" }).append(
                                $("<div/>").addClass("message-container").append(
                                    $("<div/>").addClass("subject").html(item.Subject)
                                ).append(
                                    $("<div/>").addClass("body").html(item.Body)
                                ).append(
                                    $("<div/>").addClass("control").append(getMessageControls(item)).append($("<div/>").addClass("control-message"))
                                )
                            )
                        );
                    });
                    viewFirst();
                    list.show();
                }
            });

            $this.on("click", ".view-recipients", function (e) {
                var self = $(this);
                self.prop("disabled", true);
                getRecipients(function (data) {
                    $(".recipients-list", $this).html("");
                    $.each(data, function (index, item) {
                        $(".recipients-list", $this).append($("<div/>").addClass("recipient-item").html(item.DisplayName));
                    });
                    $(".recipients-list", $this).show();
                    self.prop("disabled", false);
                });
            }).on("click", ".send-message", function (e) {
                var self = $(this);
                self.prop("disabled", true);
                sendMessage(function (data) {
                    var s = (data == 1) ? "" : "s";
                    $(".control-message", $this).html("").append($("<div/>").addClass("send-success").html(data + " message" + s + " sent."))
                    self.prop("disabled", false);
                }, function (err) {
                    self.prop("disabled", false);
                });
            });
        });
    }
}(jQuery))