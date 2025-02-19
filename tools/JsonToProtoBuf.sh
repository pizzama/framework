project_path="$(dirname "$PWD")"
echo "project path is: $project_path"
target_path="$project_path/Document/Protobuf"
file_path="$project_path/Document/Json"
tools_path="$project_path/tools"
unity_path="$project_path/Assets"

echo $project_path
echo $target_path
echo $file_path
echo $tools_path
echo $unity_path

./main -t $target_path -s $file_path -l $tools_path -m 3 -d true

# 复制 bytes 目录内容（不包含 bytes 目录本身）
cp -r "$target_path/bytes/"* "$unity_path/StreamingAssets/bytes/"

# 复制 csharp 目录内容（不包含 csharp 目录本身）
cp -r "$target_path/csharp/"* "$unity_path/Script/Config/"

# 复制 bytes 目录内容（不包含 bytes 目录本身）
#rsync -av "$target_path/bytes/" "$unity_path/StreamingAssets/bytes/"

# 复制 csharp 目录内容（不包含 csharp 目录本身）
#rsync -av "$target_path/csharp/" "$unity_path/Script/Config/"
