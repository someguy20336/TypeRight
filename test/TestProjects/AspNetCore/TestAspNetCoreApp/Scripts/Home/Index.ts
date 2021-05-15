import * as api from "./CustomNamedActions.js";

let textArea = document.getElementById("result") as HTMLTextAreaElement;
let fromQuerySimpleTest = document.getElementById("fromQuerySimpleTest");
let fromQueryComplexTest = document.getElementById("fromQueryComplexTest");
let fromQueryComplexArrayTest = document.getElementById("fromQueryComplexArrayTest");

fromQuerySimpleTest.addEventListener("click", async () => {
    let res = await api.withFromQuery("1234", {
        stringProp: "abc 123",
        genericArg: null,
        obj1: null
    });

    textArea.value = JSON.stringify(res);
});

fromQueryComplexTest.addEventListener("click", async () => {
    let res = await api.complexFromQuery("theID", {
        propOne: 1234,
        propTwo: "string value!"     
    });

    textArea.value = JSON.stringify(res);
});

fromQueryComplexArrayTest.addEventListener("click", async () => {
    let res = await api.complexWithListFromQuery({
        simpleType: "string val",
        arrayType: [1, 2, 3, 4]
    });

    textArea.value = JSON.stringify(res);
});