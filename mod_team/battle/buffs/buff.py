from blueprint.core.config import Configs
import importlib
import random


class BuffFactory:
    def __init__(self):
        pass

    def create(self, src, name, params):
        if name:
            tname = "buffs.%s" % name
            module = importlib.import_module(tname)
            BufClass = getattr(module, "%s" % name)
            buff_id = params.get("buff_id")
            if buff_id:
                buf = BufClass(buff_id, params)
                buf.init()
                buf.set_src(src)
                return buf
            print("not assign buff_id")

        return None


class Buff:
    def __init__(self, buff_id, params):
        self.configs = Configs.CONFIGS
        self.buff_id = buff_id
        self.src = None  # 释放buf的人
        self.target = None  # 被buff附加的统帅
        self.value = 0
        self.sort = 0
        # 处理buff_id对应的配置
        self.init_config()
        for k in params:
            setattr(self, k, params[k])

    def init_config(self):
        confs = self.configs.get("skill_detail.buff_detail")
        conf = confs[confs["buff_id"] == self.buff_id]
        if conf.empty:
            self.count = 0  # 触发次数
            self.btype = 0  # buff类型 1,叠加 2,替换  3, 高值替换低值, 4 永远存在不会替换
            self.bgroup = 0  # buff组 只有同一组的buff才能叠加和替换
        else:
            self.btype = conf['buff_type'].array[0]
            self.bgroup = conf['buff_group'].array[0]
            self.count = conf['duration'].array[0]

    def init(self):
        pass

    def set_target(self, target):
        self.target = target

    def set_src(self, src):
        self.src = src

    def pre_execute(self):  # 我出手前触发
        pass

    def aft_execute(self, battlelog):  # 我出手后触发
        pass

    def add_execute(self):  # 添加buff时触发
        pass

    def hurt_execute(self, skill, hurt, total):  # 受伤害时触发, 会替换原来的伤害. hurt 为原来伤害，total最终总伤害
        return 0

    def care_group(self):
        return []

    def destory(self):  # buff结束
        pass

    def get_name(self):
        return self.__class__.__name__

    def set_count(self, value):
        self.count = value

    def is_avaliable(self):
        return self.count > 0

    def del_count(self):
        self.count -= 1
        if self.count < 0:
            self.count = 0

    def is_effect(self, master, enemy):
        if enemy.is_dead():
            return False
        maxvalue = max((master.effectshit - enemy.effectsdef) / 10, 40)
        rt = min(maxvalue, 100)
        rad = random.randint(40, 100)
        if rt >= rad:
            return True
        else:
            return False
