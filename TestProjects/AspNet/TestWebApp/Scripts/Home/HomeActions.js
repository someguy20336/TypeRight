// File Autogenerated by TypeRight.  DO NOT EDIT
var WebMethods;
(function (WebMethods) {
    var HomeController;
    (function (HomeController) {
        /**
         *
         * @param dict
         */
        function Test_Anonymous(dict) {
            Comm.callService("/Home/Test_Anonymous", { dict: dict });
        }
        HomeController.Test_Anonymous = Test_Anonymous;
        /**
         *
         * @param example
         */
        function Test_ObjectParam(example) {
            Comm.callService("/Home/Test_ObjectParam", { example: example });
        }
        HomeController.Test_ObjectParam = Test_ObjectParam;
        /**
         *
         * @param dict
         */
        function Test_ObjectReturn(dict) {
            Comm.callService("/Home/Test_ObjectReturn", { dict: dict });
        }
        HomeController.Test_ObjectReturn = Test_ObjectReturn;
        /**
         *
         */
        function TestJson() {
            Comm.callService("/Home/TestJson", {});
        }
        HomeController.TestJson = TestJson;
    })(HomeController = WebMethods.HomeController || (WebMethods.HomeController = {}));
})(WebMethods || (WebMethods = {}));
//# sourceMappingURL=HomeActions.js.map