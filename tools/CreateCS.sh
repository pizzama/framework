#!/bin/bash

project_path="$(dirname "$PWD")
echo "project path is: $project_path"

target_path="$project_path/Document/Protobuf/"
file_path="$project_path/Document/Protobuf/protobuf"
tools_path="$project_path/tools"
copy_path="$project_path/Assets/Script/Config"

# 遍历目录下的所有文件
for dir in "$file_path"/*; do
    # 检查文件是否是.proto文件
    if [[ "$dir" == *.proto ]]; then
        echo "Processing $dir"
        # 使用protoc编译.proto文件
        if ! "$tools_path/protoc-23.0/osx/bin/protoc" --csharp_out="$target_path" --proto_path="$file_path" "$dir"; then
            echo "Failed to process $dir"
        fi
    fi
done

# 检查目标目录是否存在
if [ ! -d "$copy_path" ]; then
    echo "Error: Target directory $copy_path does not exist."
    exit 1
fi

# 检查目标目录是否可写
if [ ! -w "$copy_path" ]; then
    echo "Error: Target directory $copy_path is not writable."
    exit 1
fi

# 遍历目标目录下的所有.cs文件
for file in "$target_path"/*.cs; do
    # 提取文件名（不带路径）
    filename=$(basename "$file")
    # 复制文件到目标目录
    if cp "$file" "$copy_path/$filename"; then
        echo "Copied $file to $copy_path/$filename"
    else
        echo "Failed to copy $file to $copy_path/$filename"
    fi
done
