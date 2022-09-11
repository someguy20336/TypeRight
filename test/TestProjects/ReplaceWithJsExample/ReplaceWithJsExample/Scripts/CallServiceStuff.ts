

export function fetchWrapper<T>(method: string, url: string, body: any, abortSignal?: AbortSignal): Promise<T> {

	let data: string = null;
	if (body) {
		data = JSON.stringify(body);
    }

	return new Promise<T>((resolve, reject) => {
		fetch(url, {
			method: method,
			headers: {
				"Content-Type": "application/json; charset=utf-8",
			},
			body: data,
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
