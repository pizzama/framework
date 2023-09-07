#!/bin/bash
target_path=/Volumes/work/flexmobi/beangel/Assets/Script/Config/
file_path=/Volumes/work/flexmobi/beangel/Document/Protocol/
tools_path=/Volumes/work/flexmobi/excelCovertXml/tools

for dir in $(ls $file_path)
do
	echo $file_path$dir
	$tools_path/protoc-23.0/osx/bin/protoc  --csharp_out=$target_path --proto_path=$file_path $dir
done
