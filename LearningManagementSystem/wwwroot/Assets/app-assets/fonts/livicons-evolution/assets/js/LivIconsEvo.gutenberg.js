(function(){
    "use strict";
    if (window.wp.element && window.wp.richText && window.wp.i18n && window.wp.editor) {
        const { __ } = window.wp.i18n;
        const { createElement, Fragment } = window.wp.element;
        const { registerFormatType } = window.wp.richText;
        const { RichTextToolbarButton } = window.wp.editor;
        const type = 'livicons-evolution/add-livicon';
        registerFormatType(type, {
            title: __('Copy-Paste LivIcon', 'livicons-evo'),
            tagName: 'livicon',
            className: null,
            edit () {
                return (
                    createElement(Fragment, null,
                        createElement(RichTextToolbarButton, {
                            icon: 'plus-alt',
                            title: __('Copy-Paste LivIcon', 'livicons-evo'),
                            onClick: function () {
                                jQuery('.lievo-gutenberg').trigger('click');
                            }
                        })
                    )
                );
            }
        });
    }
})();
