window.jQuery(document).ready(function($) {

	"use strict";

	$('.lievo-color-picker').wpColorPicker({
		defaultColor: function () {
			return $(this).data('savedcolor');
		},
		mode: 'hsl',
		hide: true,
		width: 300,
		palettes: false
	});

	$('#lievo_admin_form').on('submit', function() {
		if ( $('#submit').hasClass('lievo-btn-deactivate') ) {
			var c = window.confirm(livicons_evolution_confirms_in_settings.deactivate_code_msg);
			return c;
		}
	});

	$("#delete_options_on_uninstall").on("change", function() {
		var that = $(this),
			c;
		if( that.is(':checked') ) {
			c = window.confirm(livicons_evolution_confirms_in_settings.delete_options_msg);
			if (c === true) {
				that.attr('checked', 'checked');
			} else {
				that.removeAttr('checked');
			}
		}
	});
		
	$('#sizeUnits').on('change', function () {
		var val = $(this).val();
		if ( val === 'px' ) {
			var form = $('#size').val();
			$('#size').attr({min:1, step:1});
			$('#size').val(Math.round(form));
		} else {
			$('#size').attr({min:0.1, step:0.1});
		}
	});
	$('#sizeUnits').change();

	$('#customStrokeWidth').on('focus', function () {
		$('#strokeWidth').prop('checked', true);
	});

	$('#customRotate').on('focus', function () {
		$('#rotate').prop('checked', true);
	});

	$('#colorsOnHoverHue').on('focus', function () {
		$('#colorsOnHover').prop('checked', true);
	});

	$('#colorsWhenMorphHue').on('focus', function () {
		$('#colorsWhenMorph').prop('checked', true);
	});

	$('#morphImageUrl').on('focus', function () {
		$('#morphImage').prop('checked', true);
	});

	$('#strokeWidthFactorOnHoverValue').on('focus', function () {
		$('#strokeWidthFactorOnHover').prop('checked', true);
	});

	$('#eventOnElem').on('focus', function () {
		$('#eventOn').prop('checked', true);
	});

	$('#customDuration').on('focus', function () {
		$('#duration').prop('checked', true);
	});

	$('#customRepeat').on('focus', function () {
		$('#repeat').prop('checked', true);
	});

	$('#customRepeatDelay').on('focus', function () {
		$('#repeatDelay').prop('checked', true);
	});

	$('#customDrawColor .wp-picker-container').on('click', function () {
		$('#customValueDrawColor').prop('checked', true);
	});

	var editorSettings,
		editor,
		_ = window._,
		wp = window.wp;
	if( $('#lievo_custom_js').length ) {
		editorSettings = wp.codeEditor.defaultSettings ? _.clone( wp.codeEditor.defaultSettings ) : {};
		editorSettings.codemirror = _.extend(
			{},
			editorSettings.codemirror,
			{
				indentUnit: 2,
				tabSize: 2,
				mode: 'javascript',
			}
		);
		editor = wp.codeEditor.initialize( $('#lievo_custom_js'), editorSettings );
	}

	if( $('#lievo_custom_css').length ) {
		editorSettings = wp.codeEditor.defaultSettings ? _.clone( wp.codeEditor.defaultSettings ) : {};
		editorSettings.codemirror = _.extend(
			{},
			editorSettings.codemirror,
			{
				indentUnit: 2,
				tabSize: 2,
				mode: 'css',
			}
		);
		editor = wp.codeEditor.initialize( $('#lievo_custom_css'), editorSettings );
	}
});