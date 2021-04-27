import * as api from "./CustomNamedActions.js";

let textArea = document.getElementById("result") as HTMLTextAreaElement;
let fromQuerySimpleTest = document.getElementById("fromQuerySimpleTest");
let fromQueryComplexTest = document.getElementById("fromQueryComplexTest");
let fromQueryComplexArrayTest = document.getElementById("fromQueryComplexArrayTest");

fromQuerySimpleTest.addEventListener("click", async () => {
    let res = await api.WithFromQuery("1234", {
        StringProp: "abc 123",
        GenericArg: null,
        Obj1: null
    });

    textArea.value = JSON.stringify(res);
});

fromQueryComplexTest.addEventListener("click", async () => {
    let res = await api.ComplexFromQuery("theID", {
        PropOne: 1234,
        PropTwo: "string value!"     
    });

    textArea.value = JSON.stringify(res);
});

fromQueryComplexArrayTest.addEventListener("click", async () => {
    let res = await api.ComplexWithListFromQuery({
        SimpleType: "string val",
        ArrayType: [1, 2, 3, 4]
    });

    textArea.value = JSON.stringify(res);
});