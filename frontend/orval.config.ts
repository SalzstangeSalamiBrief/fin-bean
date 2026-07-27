import { defineConfig } from "orval";

export default defineConfig({
  petstore: {
    output: {
      mode: "tags-split",
      target: "src/clients/generated/client.ts",
      schemas: "src/clients/generated/",
      client: "fetch",
      // TODO BASE URL
      baseUrl: "http://localhost:3000",
      mock: false,
      override: {
        enumGenerationType: "enum",
      },
    },
    input: {
      // TODO URL OF THE SPEC FILE
      target: "http://localhost:3000/openapi.yaml",
    },
  },
});
