// Header dropdown closure
(function () {
    $(document).on('mouseleave', '.header-navigation .dropdown', function () {
        $(this).removeClass('open');
    });
}());

// Alerts fading & closing
(function () {
    $('.alerts div.alert').each(function () {
        var alert = $(this);

        if (alert.data('fadeout-after') != null && alert.data('fadeout-after') != 0) {
            setTimeout(function () {
                alert.fadeTo(300, 0).slideUp(300, function () {
                    $(this).remove();
                });
            }, alert.data('fadeout-after'));
        }
    });

    $(document).on('click', '.alert .close', function () {
        $(this.parentNode.parentNode).fadeTo(300, 0).slideUp(300, function () {
            $(this).remove();
        });

        return false;
    });
}());

// Globalized validation binding
(function () {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || Globalize.parseDate(value);
    };

    $.validator.methods.number = function (value, element) {
        var pattern = new RegExp('^(?=.*\\d+.*)[-+]?\\d*[' + Globalize.culture().numberFormat['.'] + ']?\\d*$');

        return this.optional(element) || pattern.test(value);
    };

    $.validator.methods.min = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) >= parseFloat(param);
    };

    $.validator.methods.max = function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) <= parseFloat(param);
    };

    $.validator.methods.range = function (value, element, param) {
        return this.optional(element) || (Globalize.parseFloat(value) >= parseFloat(param[0]) && Globalize.parseFloat(value) <= parseFloat(param[1]));
    };

    $.validator.addMethod('greater', function (value, element, param) {
        return this.optional(element) || Globalize.parseFloat(value) > parseFloat(param);
    });
    $.validator.unobtrusive.adapters.add("greater", ["min"], function (options) {
        options.rules["greater"] = options.params.min;
        if (options.message) {
            options.messages["greater"] = options.message;
        }
    });

    $.validator.addMethod('integer', function (value, element) {
        return this.optional(element) || /^[+-]?\d+$/.test(value);
    });
    $.validator.unobtrusive.adapters.addBool("integer");

    $(document).on('change', '.datalist-hidden-input', function () {
        var validator = $(this).parents('form').validate();

        if (validator) {
            var datalistInput = $(this).prevAll('[data-datalist-for="' + this.id + '"]');
            if (validator.element('#' + this.id)) {
                datalistInput.removeClass('input-validation-error');
            } else {
                datalistInput.addClass('input-validation-error');
            }
        }
    });
    $('form').on('invalid-form', function (form, validator) {
        var datalists = $(this).find('.datalist-input');
        for (var i = 0; i < datalists.length; i++) {
            var datalistInput = $(datalists[i]);
            var hiddenInputId = datalistInput.attr('data-datalist-for');

            if (validator.invalid[hiddenInputId]) {
                datalistInput.addClass('input-validation-error');
            } else {
                datalistInput.removeClass('input-validation-error');
            }
        }
    });
    $(document).on('ready', function () {
        var hiddenDatalistInputs = $('.datalist-hidden-input.input-validation-error');
        for (var i = 0; i < hiddenDatalistInputs.length; i++) {
            var hiddenInput = $(hiddenDatalistInputs[i]);
            hiddenInput.prevAll('[data-datalist-for="' + hiddenDatalistInputs[i].id + '"]').addClass('input-validation-error');
        }
    });

    var currentIgnore = $.validator.defaults.ignore;
    $.validator.setDefaults({
        ignore: function () {
            return $(this).is(currentIgnore) && !$(this).hasClass('datalist-hidden-input');
        }
    });
}());

// Datepicker binding
(function () {
    var datePickers = $(".datepicker");
    for (var i = 0; i < datePickers.length; i++) {
        $(datePickers[i]).datepicker({
            beforeShow: function (e) {
                return !$(e).attr('readonly');
            }
        });
    }

    var datetimePickers = $(".datetimepicker");
    for (i = 0; i < datetimePickers.length; i++) {
        $(datetimePickers[i]).datetimepicker({
            beforeShow: function (e) {
                return !$(e).attr('readonly');
            }
        });
    }
}());

// JsTree binding
(function () {
    var jsTrees = $('.js-tree-view');
    for (var i = 0; i < jsTrees.length; i++) {
        var jsTree = $(jsTrees[i]).jstree({
            'core': {
                'themes': {
                    'icons': false
                }
            },
            'plugins': [
                'checkbox'
            ],
            'checkbox': {
                'keep_selected_style': false
            }
        });

        jsTree.on('ready.jstree', function (e, data) {
            var selectedNodes = $(this).prev('.js-tree-view-ids').children();
            for (var j = 0; j < selectedNodes.length; j++) {
                data.instance.select_node(selectedNodes[j].value, false, true);
            }

            data.instance.element.show();
        });
    }

    $(document).on('submit', 'form', function () {
        var jsTrees = $(this).find('.js-tree-view');
        for (var i = 0; i < jsTrees.length; i++) {
            var jsTree = $(jsTrees[i]).jstree();
            var treeIdSpan = jsTree.element.prev('.js-tree-view-ids');

            treeIdSpan.empty();
            var selectedNodes = jsTree.get_selected();
            for (var j = 0; j < selectedNodes.length; j++) {
                var node = jsTree.get_node(selectedNodes[j]);
                if (node.li_attr.id) {
                    treeIdSpan.append('<input type="hidden" value="' + node.li_attr.id + '" name="' + jsTree.element.attr('for') + '" />');
                }
            }
        }
    });
}());

// Mvc.Grid binding
(function () {
    if (window.MvcGridNumberFilter) {
        MvcGridNumberFilter.prototype.isValid = function (value) {
            var pattern = new RegExp('^(?=.*\\d+.*)[-+]?\\d*[' + Globalize.culture().numberFormat['.'] + ']?\\d*$');

            return value == '' || pattern.test(value);
        }
    }

    var mvcGrids = $('.mvc-grid');
    for (var i = 0; i < mvcGrids.length; i++) {
        $(mvcGrids[i]).mvcgrid();
    }
}());

// Input focus binding
(function () {
    var invalidInput = $('.content-container .input-validation-error:visible:not([readonly],.datepicker,.datetimepicker):first');
    if (invalidInput.length > 0) {
        invalidInput.focus();
        invalidInput.val(invalidInput.val());
    } else {
        var input = $('.content-container input:text:visible:not([readonly],.datepicker,.datetimepicker):first');
        if (input.length > 0) {
            input.focus();
            input.val(input.val());
        }
    }
}());

// Bootstrap binding
(function () {
    $('[data-toggle=tooltip]').tooltip();
}());
