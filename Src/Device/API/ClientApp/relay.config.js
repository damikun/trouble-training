module.exports = {
  src: "./src",
  schema: "./src/schema.graphql",
  exclude: ["**/node_modules/**", "**/queries/**", "**/__generated__/**"],
  extensions: ["ts", "tsx"],
  language: "typescript",
  customScalars: {
    DateTime: "String",
  },
};
