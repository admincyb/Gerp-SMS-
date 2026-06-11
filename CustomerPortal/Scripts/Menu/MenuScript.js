
        $(function () {
            // BUTTONS
            $('.fg-button').hover(
    		function () { $(this).removeClass('ui-state-default').addClass('ui-state-focus'); },
    		function () { $(this).removeClass('ui-state-focus').addClass('ui-state-default'); }
    	);

            // MENUS    	
            $('#flat').menu1({
                content: $('#flat').next().html(), // grab content from this page
                showSpeed: 400
            });

            $('#hierarchy').menu1({
                content: $('#hierarchy').next().html(),
                crumbDefaultText: ' '
            });

            $('#hierarchybreadcrumb').menu1({
                content: $('#hierarchybreadcrumb').next().html(),
                backLink: false
            });

//            // or from an external source
//            $.get('menuContent.html', function (data) { // grab content from another page
//                $('#flyout').menu1({ content: data, flyOut: true });
//            });
        });
