var WEBVIEW_CONFIG = {
    requiresAuth: false,
    path: '/test/webview'
};

var WEBVIEW_COM = Vue.component('webview', {
    mixins: [_MIXIN, _COMS],
    data: function () {
        var data = {
            _name: 'webview'
        };
        return data;
    },
    template: _apiGet('view/test/webview/index.html'),
    mounted: function () {
        document.title = 'WEBVIEW';
        $(".alert").alert('close');
    }
});