# -*- coding: utf-8 -*-
import copy
import importlib
from importlib import reload
import sys
import os
from game.core.mods.mod_team.battle.buffs.buff import BuffFactory


# 战斗中的统帅
class Card:
    def __init__(self, side, index, character):
        self.init_property(character)
        self.__index = index  # 索引位置
        self.__side = side  # 攻击或者是防守方
        self.buffs = []  # buff效果
        self.params = {}  # 特殊效果字典, 有策划决定key和value的作用
        self.character = character
        self.damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        self.resist_damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        self.exp_buff = 0

    def init_property(self, character):
        # 初始化普通攻击脚本
        self.normal_atk_script = character.normal_atk_script()
        self.normal_atk_script.init_battel()

        # 初始化技能脚本
        self.skills = character.get_activate_skill()
        for sk in self.skills:
            sk.init_battel()
        propertys = character.propertys
        self.is_leader = False  # 是否是队长
        if hasattr(character, 'is_leader'):
            self.is_leader = character.is_leader or False
        self.loco = False  # 是否是车头
        if hasattr(character, 'loco'):
            self.loco = character.loco or False
        self.nickname = ''
        if hasattr(character, 'nickname'):
            self.nickname = character.nickname or ''
        self.role_id = 0
        if hasattr(character, 'role_id'):
            self.role_id = character.role_id
        self.guild_name = ''
        if hasattr(character, 'guild_name'):
            self.guild_name = character.guild_name or ''
        # todo 所有位置必须增加stratum
        self.stratum = 1
        if hasattr(character, 'stratum'):
            self.stratum = character.stratum
        else:
            print("card do not have attr stratum")
        self.name = character.get_partner_name()  # 角色名字
        self.id = character.instance_id
        self.level = character.get_level()
        self.country = character.get_country()
        self.config_id = character.config_id
        self.is_monster = character.is_monster()
        self.fatigueVule = character.propertys.fatigueVule
        self.hp = int(propertys.hp)  # 血量
        self.maxhp = self.hp  # 血量上限
        self.attack = propertys.atk  # 攻击力
        self.defen = propertys.defend  # 防御
        self.speed = propertys.speed  # 出手速度
        self.crit = propertys.crit  # 暴击等级
        self.critimes = propertys.critimes  # 暴击伤害
        self.defcrit = propertys.defcrit  # 韧性等级
        self.treatcrit = propertys.treatcrit  # 治疗会心系数
        self.revovery = propertys.revovery  # 伤害减免
        self.damage = propertys.damage  # 伤害加成
        self.effectsdef = propertys.effectsdef  # 定立（控制抵抗）
        self.effectshit = propertys.effectshit  # 控制（控制命中）
        self.maxdander = 800  # 能量
        self.monsterDamage = propertys.monsterDamage  # 怪物伤害加成
        self.fatigueVule = propertys.fatigueVule  # 疲劳值
        self.recordhp = self.hp  # 临时记录的血量，为了疲劳值计算
        self.talent = character.get_talent_effects()

        self.copy_property()

    def record_hp(self):
        self.recordhp = self.hp

    def get_talent(self, talent_ids):
        arr = []
        for tt in self.talent:
            if tt[2] in talent_ids:
                arr.append(tt)
        return arr

    def set_speed(self, speed):
        self.speed = speed

    def set_atk(self, atk):
        if atk <= 0:
            atk = 0
        self.attack = atk

    def set_def(self, defend):
        self.defen = defend

    def set_maxhp(self, maxhp):
        self.maxhp = maxhp
        if self.hp > self.maxhp:
            self.hp = self.maxhp

    def set_crit(self, crit):
        self.crit = crit

    def set_defcrit(self, defcrit):
        self.defcrit = defcrit

    def set_effectshit(self, effectshit):
        self.effectshit = effectshit

    def set_effectsdef(self, effectsdef):
        self.effectsdef = effectsdef

    def set_maxdander(self, maxdander):
        self.maxdander = maxdander

    def get_card_name(self):
        return self.name + ":::" + self.id

    def copy_property(self):
        self.default_attack = self.attack
        self.default_defen = self.defen  # 防御
        self.default_speed = self.speed  # 出手速度
        self.default_crit = self.crit  # 暴击等级
        self.default_critimes = self.critimes  # 暴击伤害
        self.default_defcrit = self.defcrit  # 韧性等级
        self.default_treatcrit = self.treatcrit  # 治疗会心系数
        self.default_revovery = self.revovery  # 伤害减免
        self.default_damage = self.damage  # 伤害加成
        self.default_effectsdef = self.effectsdef  # 定立（控制抵抗）
        self.default_effectshit = self.effectshit  # 控制（控制命中）
        self.default_maxdander = 0  # 能量
        self.default_monsterDamage = self.monsterDamage  # 怪物伤害加成
        self.default_fatigueVule = self.fatigueVule  # 疲劳值

    def reset_property(self):
        self.attack = self.default_attack
        self.defen = self.default_defen  # 防御
        self.speed = self.default_speed  # 出手速度
        self.crit = self.default_crit  # 暴击等级
        self.critimes = self.default_critimes  # 暴击伤害
        self.defcrit = self.default_defcrit  # 韧性等级
        self.treatcrit = self.default_treatcrit  # 治疗会心系数
        self.revovery = self.default_revovery  # 伤害减免
        self.damage = self.default_damage  # 伤害加成
        self.effectsdef = self.default_effectsdef  # 定立（控制抵抗）
        self.effectshit = self.default_effectshit  # 控制（控制命中）
        self.maxdander = 0  # 能量
        self.monsterDamage = self.default_monsterDamage  # 怪物伤害加成
        self.fatigueVule = self.default_fatigueVule  # 疲劳值
        self.recordhp = self.hp  # 记录当前的血量

    @property
    def index(self):
        return self.__index

    @property
    def side(self):
        return self.__side

    def get_activite_skill_by_config(self, config_id):
        skills = self.skills
        sk = None
        for skill in skills:
            if skill.config_id == config_id:
                sk = skill
        return sk

    def skill_trigger(self, tp, atks, defens, battlelog):
        path = os.path.split(os.path.realpath(__file__))[0]
        sys.path.append(path)
        particular_skill, exchange_skill = self.character.particular_skill()
        for sk in self.skills:
            if sk.get_type() == tp:
                skname = sk.get_script()
                name = "skills.%s" % skname
                if particular_skill:
                    if name.find(exchange_skill) > 0:
                        skname = "skill_" + particular_skill + "1"
                        name = "skills.%s" % skname

                module = importlib.import_module(name)
                reload(module)
                SKClass = getattr(module, skname)
                script = SKClass(self, sk, atks, defens, battlelog)
                return script, sk

        return None, None

    # 战前技
    def skill_before_battle(self, atks, defens, battlelog):
        self.atks = atks
        self.defens = defens
        self.battlelog = battlelog
        rt = self.pre_trigger_buff(battlelog)
        if rt["sleep"] == 0 and rt["silent"] == 0:
            for card in defens:
                print("skill_before_battle", card.id)
            script, _ = self.skill_trigger(3, atks, defens, battlelog)
            if script:
                script.pre_execute()
                script.finish()

    def start_battle(self, atks, defens, battlelog):  # 开始攻击
        self.atks = atks
        self.defens = defens
        self.battlelog = battlelog
        # 进行天赋90检查
        arr = self.get_talent([90])
        for talent in arr:
            percent = talent[4]
            self.create_buff("buf_atk", {
                "buff_id": "11001",
                "value": self.attack,
                "count": 1,
            })
        rt = self.pre_trigger_buff(battlelog)
        if rt["sleep"] == 0:
            script, _ = self.skill_trigger(2, atks, defens, battlelog)
            if script:
                script.pre_execute()
                script.finish()

            script, sk = self.skill_trigger(1, atks, defens, battlelog)
            if script and self.maxdander >= sk.get_cost() and rt["silent"] == 0:
                self.maxdander = 0
                script.pre_execute()
                script.finish()
            else:
                # 判断是否有嘲讽对象
                target = rt["taunt"]
                newdefens = None
                if target:
                    for den in defens:
                        if str(den) == str(target):
                            if not den.is_dead():
                                newdefens = [den]
                if newdefens:
                    defens = newdefens
                # 普通攻击
                sk = self.normal_atk_script
                path = os.path.split(os.path.realpath(__file__))[0]
                sys.path.append(path)
                name = "skills.%s" % sk.get_script()
                module = importlib.import_module(name)
                SKClass = getattr(module, sk.get_script())
                script = SKClass(self, sk, atks, defens, battlelog)
                script.pre_execute()
                script.finish()
        self.aft_trigger_buff(battlelog)

    def hurt(self, skill, num):  # 加血和减血时触发buff
        if num < 0:
            # 进行天赋84,86检查
            arr = self.get_talent([84, 86])
            for talent in arr:
                if talent[2] == 84:
                    percent = talent[4]
                    num = int(num * (percent / 100))
                if talent[2] == 86:  # 受到治疗添加攻击力上升的buf
                    percent = talent[4]
                    num = int(num * (percent / 100))
                    self.create_buff("buf_atk", {
                        "buff_id": "11001",
                        "value": self.attack * num,
                        "count": 1,
                    })

        if num > 0:
            # 进行伤害天赋95检查
            arr = self.get_talent([95])
            for talent in arr:
                if talent[2] == 95:
                    print(talent)

        total = num  # 伤害会经过所有的buff处理
        for buff in self.buffs:
            rt = buff.hurt_execute(skill, num, total)
            total += rt
            if rt == 9999999999999:  # 处于无敌状态，不再计算buff
                return 0
        self.hp = self.hp - total
        self.hp = int(self.hp)
        if self.hp < 0:
            self.hp = 0

        if self.hp >= self.maxhp:
            self.hp = self.maxhp
        return total

    def base_hurt(self, num):  # 基础伤害,不检查附带buff
        self.hp = self.hp - num
        self.hp = int(self.hp)
        if self.hp < 0:
            self.hp = 0

        if self.hp >= self.maxhp:
            self.hp = self.maxhp
        return num

    def is_dead(self):
        return self.hp <= 0

    def add_buff(self, buff):
        if buff:
            care_group = []
            for bf in self.buffs:
                care_group += bf.care_group()
            # 无敌状态免疫所有buff
            if 0 in care_group:
                return
            if buff.bgroup in care_group:
                return

            oldbuf = self.has_same_buff(buff)
            if oldbuf:
                oldbuf.destory()

            buff.set_target(self)
            buff.add_execute()
            self.buffs.append(buff)
            self.buffs.sort(key=lambda x: x.sort)  # 升序

    def pre_trigger_buff(self, battlelog):
        self.check_buff(battlelog)
        dict = {"sleep": 0, "silent": 0, "taunt": None}
        for buff in self.buffs:
            rt = buff.pre_execute()
            if rt:
                dict.update(rt)
        return dict

    def aft_trigger_buff(self, battlelog):
        self.check_buff(battlelog)
        for buff in self.buffs:
            buff.aft_execute(battlelog)
            buff.del_count()

    def remove_buff(self, buff_ids, battlelog):
        arr = []
        for buff in self.buffs:
            if buff.buff_id in buff_ids:
                arr.append(buff)

        for index in range(len(arr) - 1, -1, -1):
            buff = self.buffs[index]
            buff.destory()
            self.buffs.remove(buff)
            battlelog.buff_del_record(buff)

    def check_buff(self, battlelog):  # 检查buff
        for buff in self.buffs:
            if not buff.is_avaliable():
                buff.destory()
                self.buffs.remove(buff)
                battlelog.buff_del_record(buff)

    def del_buff_name(self, name):  # 删除buff
        for buff in self.buffs:
            if buff.get_name() == name:
                self.buffs.remove(buff)

    def has_same_buff(self, bf):
        for buff in self.buffs:
            if bf.bgroup == buff.bgroup:
                if bf.btype == 2:
                    self.buffs.remove(buff)
                    return buff
                if bf.btype == 3:
                    if buff.value < bf.value:
                        self.buffs.remove(buff)
                        return buff
        return None

    def to_log_dict(self):
        rt = {}
        rt['name'] = self.name
        rt['id'] = self.id
        rt['config_id'] = self.config_id
        rt['hp'] = int(self.hp)
        rt['maxhp'] = int(self.maxhp)
        rt['fatigueVule'] = int(self.fatigueVule)
        rt['level'] = int(self.level)
        return rt

    def serialize(self):
        rt = {}
        # rt['name'] = self.name
        rt['id'] = self.id
        rt['config_id'] = self.config_id
        rt['hp'] = int(self.hp)
        rt['maxhp'] = int(self.maxhp)
        rt['level'] = int(self.level)
        rt['atk'] = int(self.attack)
        rt['defend'] = int(self.defen)
        rt['speed'] = int(self.speed)
        rt['crit'] = int(self.crit)
        rt['critimes'] = int(self.critimes)
        rt['defcrit'] = int(self.defcrit)
        rt['treatcrit'] = int(self.treatcrit)
        rt['revovery'] = int(self.revovery)
        rt['damage'] = int(self.damage)
        rt['effectsdef'] = int(self.effectsdef)
        rt['effectshit'] = int(self.effectshit)
        rt['maxdander'] = self.maxdander
        rt['is_monster'] = self.is_monster
        rt['monsterDamage'] = int(self.monsterDamage)
        rt['fatigueVule'] = int(self.fatigueVule)

        return rt

    def update_maxdander(self, value=10):
        self.maxdander += value

    def set_extraparams(self, key, value):
        self.params[key] = value

    def get_extraparams(self, key):
        value = self.params.get(key)
        if value is None:
            return 0
        else:
            return value

    def copy(self):
        return copy.deepcopy(self)

    def has_buff_id(self, buff_ids):
        arr = []
        for buff in self.buffs:
            if buff.buff_id in buff_ids:
                arr.append(buff)

        return arr

    # 卡牌自己释放了buff给自己
    def create_buff(self, name, params=None):
        buff = BuffFactory().create(self, name, params)
        self.add_buff(buff)
