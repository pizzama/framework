import os
from game.exceptions import GameException


class SimulatorMod:
    def __init__(self):
        pass

    def get_script_list(self):
        path = os.getcwd()
        totalpath = "%s/game/core/mods/mod_team/battle/skills" % (path)
        file_names = os.listdir(totalpath)

        filters = ["__init__.py", "__pycache__", "script.py", ".DS_Store","z_check.py"]

        fs = []
        sign = True
        for i in file_names:
            for j in filters:
                if i == j:
                    sign = False
                    break
            if sign:
                file = totalpath + "/" + i
                with open(file, 'r', encoding="utf-8") as f:
                    content = f.read()
                    f.close()
                dt = {"name": i, "content": content}
                fs.append(dt)
            sign = True

        return fs

    def deal_with_script_file(self, md, name, content):
        flag = False
        path = os.getcwd()
        file = "%s/game/core/mods/mod_team/battle/skills/%s" % (path, name)
        if not os.path.exists(file):
            raise GameException(status=1000, msg="文件不存在")
        check_file = "%s/game/core/mods/mod_team/battle/skills/z_check.py" % (path)
        with open(check_file, 'w', encoding='utf-8') as f:
            f.write(content)
            f.close()
        result_code = os.system('pyflakes {}'.format(check_file))
        if result_code !=0:
            raise GameException(status=100010,msg="文件语法校验未通过")
        else:
            flag = True
        if md == "deletecfg":
            os.remove(file)
            return {"md": md, "result": True}
        elif md == "editcfg":
            if flag == True:
                with open(file, 'w', encoding='utf-8') as f:
                    f.write(content)
                    f.close()


    def get_buff_list(self):
        path = os.getcwd()
        totalpath = "%s/game/core/mods/mod_team/battle/buffs" % (path)
        file_names = os.listdir(totalpath)

        filters = ["__init__.py", "__pycache__", "buff.py","z_check.py"]

        fs = []
        sign = True
        for i in file_names:
            for j in filters:
                if i == j:
                    sign = False
                    break
            if sign:
                file = totalpath + "/" + i
                with open(file, 'r', encoding="utf-8") as f:
                    content = f.read()
                    f.close()
                dt = {"name": i, "content": content}
                fs.append(dt)
            sign = True

        return fs

    def deal_with_buff_file(self, md, name, content):
        flag = False
        path = os.getcwd()
        file = "%s/game/core/mods/mod_team/battle/buffs/%s" % (path, name)
        if not os.path.exists(file):
            raise GameException(status=1000, msg="文件不存在")
        check_file = "%s/game/core/mods/mod_team/battle/buffs/z_check.py" % (path)
        with open(check_file, 'w', encoding='utf-8') as f:
            f.write(content)
            f.close()
        result_code = os.system('pyflakes {}'.format(check_file))
        if result_code !=0:
            raise GameException(status=100010,msg="文件语法校验未通过")
        else:
            flag = True
        if md == "deletecfg":
            os.remove(file)
            return {"md": md, "result": True}
        elif md == "editcfg":
            if flag == True:
                with open(file, 'w', encoding='utf-8') as f:
                    f.write(content)
                    f.close()
        