export function callService(path, p1, p2, p3) { }
/**
 * Calls a post action
 * @param url The URL to call
 * @param data the data to pass
 * @param success the success function
 * @param fail the fail function
 */
export function callPost(url, data, abortSignal) {
    return new Promise((resolve, reject) => {
        fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify(data),
            signal: abortSignal
        }).then((resp) => {
            resp.json().then(val => {
                resolve(val);
            }).catch((reason) => {
                console.log("Caught error: " + reason);
            });
        }).catch((reason) => {
            reject(reason);
        });
    });
}
//# sourceMappingURL=CallServiceStuff.js.map