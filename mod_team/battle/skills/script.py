import random
from buffs.buff import BuffFactory
from blueprint.core.config import Configs


class SklData:
    def __init__(self):
        self.des = []
        self.htp = []
        self.hurt = []
        self.eng = {}

    def add_des(self, des):
        self.des.append(des)

    def add_htp(self, htp):
        self.htp.append(htp)

    def add_hurt(self, hurt):
        hurt = int(hurt)
        self.hurt.append(hurt)

    def get_des(self):
        return self.des

    def get_htp(self):
        return self.htp

    def get_hurt(self):
        return self.hurt

    def set_eng(self, src):
        self.eng[src.id] = src.maxdander


class Script:
    def __init__(self, master, skill, atks, defens, battlelog):
        self.skill = skill
        self.sid = skill.config_id
        self.master = master
        self.atks = atks
        self.defens = defens
        self.battlelog = battlelog

    def pre_execute(self):
        # 统一记录时间
        configs = Configs.CONFIGS
        confs = configs.get('skill_action_time.action_time')
        key = "%s-%s" % (self.master.config_id, self.sid)
        conf = confs[confs['id'] == key]
        if not conf.empty:
            duration = conf['duration'].array[0]
            offsettime = conf['offsettime'].array[0]
            totaltime = duration + offsettime
            self.battlelog.caculate_time(totaltime)
        # 83 天赋检查 自我能量加成
        if self.get_skill_type() == 0:
            arr = self.master.get_talent([83])
            if (len(arr) > 0):
                for talent in arr:
                    self.master.update_maxdander(talent[4])

        self.execute()

    def execute(self):
        # muse be return SklData
        raise NotImplementedError

    def finish(self):
        self.battlelog.rls_record(self.master.id)
        temp = []
        for cha in self.defens:
            if cha.is_dead():
                temp.append(cha)
        if len(temp) > 0:
            self.battlelog.lck_record("")
            for cha in temp:
                self.create_death_log(cha)
            self.battlelog.rls_record("")

    def enemys_scope_property(self, sign, key, value):
        enemy = []
        for en in self.defens:
            if en.is_dead():
                continue
            kv = getattr(en, key)
            if sign == "=":
                if value == kv:
                    enemy.append(en)
            if sign == ">":
                if value > kv:
                    enemy.append(en)
            if sign == "<":
                if value < kv:
                    enemy.append(en)

        return enemy

    def enemys_max_property(self, key):
        enemy = None
        for he in self.defens:
            if he.is_dead():
                continue
            if not enemy:
                enemy = he
            else:
                n1 = getattr(enemy, key)
                n2 = getattr(he, key)
                if n1 < n2:
                    enemy = he

        return [enemy]

    def enemys_min_property(self, key):
        enemy = None
        for he in self.defens:
            if he.is_dead():
                continue
            if not enemy:
                enemy = he
            else:
                n1 = getattr(enemy, key)
                n2 = getattr(he, key)
                if n1 < n2:
                    enemy = he

        return [enemy]

    def enemys_target_default(self):  # 获得默认敌方目标
        idx = self.master.index
        enemy = None
        for _, en in enumerate(self.defens):
            if en.is_dead():
                continue
            if not enemy:
                enemy = en
            if en.index == idx:
                enemy = en

        return [enemy]

    def enemys_random_target(self, num):  # 随机选择敌方目标
        arr = []
        for cha in self.defens:
            if not cha.is_dead():
                arr.append(cha)

        if len(arr) < num:
            return arr

        rt = random.sample(range(0, len(arr)), num)
        enemys = []
        for idx in rt:
            enemys.append(arr[idx])

        return enemys

    def heros_random_target(self, num):  # 随机友军目标
        arr = []
        for cha in self.atks:
            if not cha.is_dead():
                arr.append(cha)
        if len(arr) < num:
            return arr

        rt = random.sample(range(0, len(arr)), num)
        heros = []
        for idx in rt:
            heros.append(arr[idx])

        return heros

    def heros_max_property(self, key):
        cha = None
        for he in self.atks:
            if he.is_dead():
                continue
            if not cha:
                cha = he
            else:
                n1 = getattr(cha, key)
                n2 = getattr(he, key)
                if n1 < n2:
                    cha = he

        return [cha]

    def heros_min_property(self, key):
        cha = None
        for he in self.atks:
            if he.is_dead():
                continue
            if not cha:
                cha = he
            else:
                n1 = getattr(cha, key)
                n2 = getattr(he, key)
                if n1 < n2:
                    cha = he

        return [cha]

    def heros_all(self):  # 友军全体
        arr = []
        for cha in self.atks:
            if not cha.is_dead():
                arr.append(cha)
        return arr

    def enemys_all(self):
        arr = []
        for cha in self.defens:
            if not cha.is_dead():
                arr.append(cha)
        return arr

    # 谁释放了buff
    def create_buff(self, src, name, params=None):
        buff = BuffFactory().create(src, name, params)
        return buff

    def delete_buff(self, buff):
        pass

    def create_buff_log(self, buff):
        if buff:
            self.battlelog.lck_record(buff.src.id)
            self.battlelog.buff_record(buff)
            self.battlelog.rls_record(buff.src.id)

    def create_death_log(self, src):
        self.battlelog.death_record(src)

    def create_skill_log(self, skldata, skill_type=None):
        print("create_skill_log, self.master", self.master.id)
        for card in skldata.des:
            print("create_skill_log, skldata.des", card.id)
        # skill_type 标记技能是普通攻击还是技能 1 # 普通攻击， 2 技能攻击
        self.battlelog.lck_record(self.master.id)
        self.battlelog.skill_record(self, self.master, skldata)
        if not skill_type:
            skill_type = 2
        self.battlelog.record_hurt_log(skldata, skill_type)
        self.battlelog.rls_record(self.master.id)

    def normal_hurt(self, enemy):  # 暴击是1 普攻是0
        hurt = max(self.master.attack - enemy.defen, self.master.attack * 0.1) * (1 + self.master.damage) * (1 - enemy.revovery)
        # 科技对角色的加成
        damage_buff = self.master.damage_buff[enemy.country]
        if damage_buff:
            hurt = int(hurt * (damage_buff[1] / 100 + 1) + damage_buff[0])
        
        rt = self.crit_hurt(enemy, hurt)
        if rt:
            return rt, 1

        return hurt, 0

    def lift_hurt(self, enemy):
        pass

    def crit_hurt(self, enemy, hurt):
        crit = max((self.master.crit - enemy.defcrit), 20) / 10
        rt = random.randint(1, 100)
        if crit >= rt:
            hurt = hurt * (1.5 + self.master.crit / 1000)
            return hurt
        return None

    def create_skl_data(self):
        return SklData()

    def get_skill_type(self):  # 返回时普通攻击还是技能攻击 0, 普通攻击, 1 技能攻击
        return 1

    def skill_buff(self, des, buff):
        if not des.is_dead():
            des.add_buff(buff)
            self.create_buff_log(buff)

    def get_my_skill_level(self, config_id):
        skill = self.master.get_activite_skill_by_config(config_id)
        if skill:
            return skill.level
        else:
            return 0

    def remove_buff(self, card, buff_ids):
        card.remove_buff(buff_ids, self.battlelog)
