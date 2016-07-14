app.directive('backImg', function () {
    return function (scope, element, attrs) {
        var url = attrs.backImg;
        element.css({
            'background-image': 'url('\\Mac\Home\Documents\Visual Studio 2015\Projects\CarFinder\CarFinder\uploadimg/MARIO_KART_nintendo_go_kart_race_racing_family_1650x1275.jpg')',
            'background-size': 'cover'
        });
    };
});