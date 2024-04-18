import requests
import os
import sys
from gevent import monkey,spawn,sleep, wait
def main():
    fixUrl()
    # getUI()
    # getScene()
    # getFlowers()


def getUI():
    path=os.path.dirname(sys.argv[0]) + "/ui/"
    errorTxt = "NoSuchKey"
    name = "npc_0_tex"
    x = range(1)
    for n in x:
        # name += 1
        filename =str(name)
        print("search file:" + filename)
        sleep(1)
        url = "https://static.fthformal.com/flower/flower_kk/ver/1/resource/assets/db/npc/0/"+ filename + ".png"
        r = requests.get(url)
        if(r.text.find(errorTxt) == -1):
            print("write file:" + filename)
            with open(path + filename + ".png", "wb") as f:
                f.write(r.content)

def getScene():
    path=os.path.dirname(sys.argv[0]) + "/scenes/"
    errorTxt = "NoSuchKey"
    name = 0
    x = range(10)
    for n in x:
        name += 1
        filename = str(name)
        print("search file:" + filename)
        sleep(1)
        # url = "https://static.fthformal.com/flower/flower_kk/ver/87/resource/assets/h5scene/" + filename + ".png"
        # url = "https://static.fthformal.com/flower/flower_kk/ver/1/resource/assets/h5scene/" + filename + ".png"
        # url = "https://static.fthformal.com/flower/flower_kk/ver/146/resource/assets/h5scene/"+ filename + ".png"
        url = "https://static.fthformal.com/flower/flower_kk/ver/186/resource/assets/map/mapV1/" + filename + ".jpg"
        
        r = requests.get(url)
        if(r.text.find(errorTxt) == -1):
            print("write file:" + filename)
            with open(path + filename + ".png", "wb") as f:
                f.write(r.content)


def getFlowers():
    path=os.path.dirname(sys.argv[0]) + "/images/"
    prefixName = "a"
    errorTxt = "NoSuchKey"
    name = 40011501
    x = range(20)
    for n in x:
        sub = 1
        name += 1
        filename = prefixName + str(name)
        print("search file:" + filename)
        while sub < 4:
            sleep(1)
            url = "https://static.fthformal.com/flower/flower_kk/ver/1/resource/assets/h5scene/" + filename + "_" + str(sub) + ".png"
            r = requests.get(url)
            if(r.text.find(errorTxt) != -1):
                break
            with open(path + filename + "_" + str(sub) + ".png", "wb") as f:
                print("write file:" + filename + "_" + str(sub))
                f.write(r.content)
            sub+= 1
            
def fixUrl():
    folder_path=os.path.dirname(sys.argv[0]) + "/"
    errorTxt = "NoSuchKey"
    url = "https://static.fthformal.com/flower/flower_kk/ver/196/version.json"
    r = requests.get(url)
    json = r.json()
    temp = json["verDic"]
    count = 0
    for path in temp:
        # sleep(1)
        ver = temp[path]
        index = path.rfind('/')
        subpath = path[0: index]
        out_path = folder_path + subpath
        url = "https://static.fthformal.com/flower/flower_kk/ver/" + str(ver) + "/" + path
        count += 1
        # r = requests.get(url)
        # if(r.text.find(errorTxt) != -1):
        #     break
        # try:
        #     os.makedirs(out_path, exist_ok=True)
        #     print(f"Folder '{out_path}' created successfully.")
        # except OSError as error:
        #     print(f"Error: {error}")
        # with open(path, "wb") as f:
        #     print("write file:" + path)
        #     f.write(r.content)

if __name__ == '__main__':
    main()