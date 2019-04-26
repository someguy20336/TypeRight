
export function callService(path: string, p1: any, p2: any, p3: any) { }

/**
 * Calls a post action
 * @param url The URL to call
 * @param data the data to pass
 * @param success the success function
 * @param fail the fail function
 */
export function callPost<T>(url: string, data: any, abortSignal?: AbortSignal): Promise<T> {

	return new Promise<T>((resolve, reject) => {
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