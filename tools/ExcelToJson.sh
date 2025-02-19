#!/bin/bash

project_path="$(dirname "$PWD")"
echo "project path is: $project_path"
target_path="$project_path/Document/Json"
file_path="$project_path/Document/Excel"
tools_path="$project_path/tools"

echo $project_path
echo $target_path
echo $file_path
./main -t $target_path -s $file_path -l $tools_path -m 1 -d true
