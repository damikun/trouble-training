#! /usr/bin/env node

const fs = require("fs");

//-----------------------------------------------
// Info
//-----------------------------------------------

/*
 - This script converts standard relay output of .json to Hotchocolate server presisted query input format
 - HC require each query to be in separate file named by Query Hash whith file type .graphql {Hash}.graphql
 
 !!! All files in output directory ending with .graphql will be first deleted (prevents old files to presist). Make shure you provide right directory!

 Put script after your standard relay generation:

  "scripts": {
    "relay": "relay-compiler --persist-output persisted-output/persisted-queries.json && node relay_converter.js persisted-output/persisted-queries.json persisted-output",
   }

   You can provide source and output argumnets: "node relay_converter.js {source_file_path} {output_directory_path}""

   default values are: "node relay_converter.js persisted-output/persisted-queries.json persisted-output"

   DONT define output folder as relay scope folder.. this directory must be out of scope defined in relay.config.js
*/

//-----------------------------------------------
// Handle input arguments
//-----------------------------------------------

//By default expect source file to be in "persisted-output" dir
let PATH_RELAY_SOURCE_FILE = "persisted-output/persisted-queries.json";
//By default output dirrectory is "persisted-output"
let PATH_OUTPUT_DIRECTORY = "persisted-output";

// Tryes gets arguments
var myArgs = process?.argv?.slice(2);

if (myArgs?.length === 1) {
  throw "Invalid number of argumenst -> must be 2 (source and destination path)";
} else if (myArgs?.length === 2) {
  // In case custom souce and output are provided will be over-set...
  PATH_RELAY_SOURCE_FILE = myArgs[0];
  PATH_OUTPUT_DIRECTORY = myArgs[1];
}

// Validate if Source file exist
if (!fs.existsSync(PATH_RELAY_SOURCE_FILE)) {
  throw new Error(`Source file: ${PATH_RELAY_SOURCE_FILE} was not found`);
}
// Validate if Output directory exist
if (!fs.existsSync(PATH_OUTPUT_DIRECTORY)) {
  throw new Error(`Output directory: ${PATH_OUTPUT_DIRECTORY} was not found`);
}

const SOURCE_FILE_NAME = PATH_RELAY_SOURCE_FILE.replace(/^.*[\\\/]/, "");

// Glob variables
var failed_generated_counter = 0;
var success_generated_counter = 0;
var skipped_delete_counter = 0;
var deleted_counter = 0;

//-----------------------------------------------
// Clean output folder
//-----------------------------------------------

const removeDir = function (path) {
  const files = fs.readdirSync(path);

  if (files && files.length > 0) {
    files.forEach(function (filename) {
      if (SOURCE_FILE_NAME === filename) return;

      let file_path = `${path}/${filename}`;

      if (fs.statSync(file_path)?.isDirectory()) {
        // removeDir(file_path);
        skipped_delete_counter++;
      } else {
        const fileName_Arr = filename.split(".");
        if (
          fileName_Arr &&
          fileName_Arr.length > 0 &&
          fileName_Arr[1] === "graphql"
        ) {
          fs.unlinkSync(file_path);
          console.log(`${filename} --> Removed`);
          deleted_counter++;
        } else {
          skipped_delete_counter++;
        }
      }
    });
  }
};

removeDir(PATH_OUTPUT_DIRECTORY);

//-----------------------------------------------
// Async files generation
//-----------------------------------------------

// Read file from Source path
let rawdata = fs.readFileSync(PATH_RELAY_SOURCE_FILE);

// Parse json to js object
let JsonObject = JSON.parse(rawdata);

const Promises = Object.keys(JsonObject).map((prop) => {
  const data_to_Write = JsonObject[prop]?.replace(/\n/g, "")?.toString();
  const file_name = prop;

  // write JSON query to a file with its query hash
  return fs.promises
    .writeFile(`${PATH_OUTPUT_DIRECTORY}/${file_name}.graphql`, data_to_Write)
    .then(() => {
      success_generated_counter++;
      console.log(`${file_name} --> OK`);
    })
    .catch((err) => {
      success_generated_counter++;
      console.log(`\n${file_name} --> FAILED \n`);
      console.log(err);
    });
});

//-----------------------------------------------
// Await and write results to console
//-----------------------------------------------

Promise.all(Promises)
  .then((_) => {
    console.log(`\n ----- CONVERSION COMPLETED ------ `);
    console.log(
      `Deleted fles: ${deleted_counter}, Skipped delete: ${skipped_delete_counter}`
    );
    console.log(
      `Generated success: ${success_generated_counter}, Generated failed: ${failed_generated_counter} \n`
    );
  })
  .catch((err) => {
    console.log(`\n ----- CONVERSION FAILED ------ \n`);
    console.error(err);
  });
