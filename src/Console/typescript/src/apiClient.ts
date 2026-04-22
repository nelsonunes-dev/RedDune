import type { SimulationInput, SimulationOutput } from "./types.js";

export interface ApiClientOptions {
  baseUrl: string;
  timeout?: number;
}

export class ApiClient {
  private baseUrl: string;
  private timeout: number;

  constructor(options: ApiClientOptions) {
    this.baseUrl = options.baseUrl.replace(/\/$/, "");
    this.timeout = options.timeout ?? 30000;
  }

  async simulate(input: SimulationInput): Promise<SimulationOutput> {
    const url = `${this.baseUrl}/api/simulation`;
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.timeout);

    try {
      const response = await fetch(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
        signal: controller.signal,
      });

      clearTimeout(timeoutId);

      if (!response.ok) {
        const errorText = await response.text().catch(() => "Unknown error");
        throw new Error(`API error ${response.status}: ${errorText}`);
      }

      const result = (await response.json()) as SimulationOutput;
      return result;
    } catch (error) {
      clearTimeout(timeoutId);
      if (error instanceof Error && error.name === "AbortError") {
        throw new Error(`Request timeout after ${this.timeout}ms`);
      }
      throw error;
    }
  }
}

export function createApiClient(baseUrl: string): ApiClient {
  return new ApiClient({ baseUrl });
}