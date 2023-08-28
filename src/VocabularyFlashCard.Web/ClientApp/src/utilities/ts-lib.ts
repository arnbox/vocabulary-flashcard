export class Ts {
	public static hasValue(value: string | object | undefined | null): boolean {
		if (typeof value !== "undefined" && value !== null) {
			if (typeof value === "string") {
				// 'true' for non-empty strings
				return value.trim() !== "";
			} else {
				// 'true' for objects with value
				return true;
			}
		}
		return false;
	}

	public static DayOfWeekFromMonday(dateString: string): number {
		const date = new Date(dateString);
		const day = date.getDay();
		return day === 0 ? 6 : day - 1;
	}

	public static DateInfo(dateString: string) {
		const result = `${dateString.substring(0, 19)} ${this.getWeekDayName(
			dateString,
		)} (${this.DiffFromNow(dateString)})`;

		return result;
	}

	public static DiffFromNow(dateString: string): string {
		const dateNow = new Date();
		const datePast = new Date(dateString);

		const diffMs = dateNow.getTime() - datePast.getTime(); // milliseconds
		const diffDays = Math.floor(diffMs / 86400000); // days
		const diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
		// const diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes

		let result = `${diffDays} ${diffDays > 1 ? "days" : "day"}, `;
		if (diffHrs > 0) {
			result += `${diffHrs} ${diffHrs > 1 ? "hours" : "hour"}`;
		} else {
			result = result.replace(",", "").trim();
		}
		// result += `${diffMins} ${diffMins > 1 ? "minutes" : "minute"}`;
		return result;
	}

	public static getWeekDayName(dateSTring: string) {
		const dayOfWeekName = new Date(dateSTring).toLocaleString("en-US", {
			weekday: "long",
		});

		return dayOfWeekName;
	}

	public static GetISOMonth(dateString = ""): string {
		let date = "";
		if (this.hasValue(dateString)) {
			date = new Date(dateString).toISOString();
		} else {
			date = new Date().toISOString();
		}

		return date.substring(0, 7);
	}

	public static RandomString(len = 10) {
		const id = [...Array(len)]
			.map(() => Math.random().toString(36)[2])
			.join("");
		return id;
	}

	public static extractNumbers(str: string) {
		return str.replace(/^\D+/g, ""); // remove all non-digits
	}

	public static FileToBase64(
		file: File,
		getRawData: boolean = true,
	): Promise<string> {
		return new Promise((resolve, reject) => {
			const reader = new FileReader();
			reader.readAsDataURL(file);
			reader.onload = () => {
				let encoded = reader.result?.toString() ?? "";
				if (getRawData) {
					encoded = encoded.replace(/^data:(.*,)?/, "") ?? "";
					if (encoded.length % 4 > 0) {
						encoded += "=".repeat(4 - (encoded.length % 4));
					}
				}
				resolve(encoded);
			};
			reader.onerror = (error) => reject(error);
		});
	}
}
