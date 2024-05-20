from blueprint.blueprints.role import Role
from game.core.mods.mod_team.battle.battle import Battle
from game.exceptions import GameException
from game.models.dbrole import DbRoleManager
from game.servers.serverstatic import AIMTYPE
import copy


class GABattle:
    def __init__(self, atkteams, defenseteams):
        self.atkteams = atkteams
        self.defenseteams = defenseteams
        self.atkbackup = copy.copy(atkteams)
        self.defenbackup = copy.copy(defenseteams)  # 给两个队列做备份
        self.battle = None
        self.round = 0
        self.first_section_win = 0
        self.battle_id = ""
        self.atk_max_fatigue, self.def_max_fatigue = self.record_current_fatigue_vule()
        self.team_record_log = {}
        self.team_record_log["log"] = []
        self.team_record_log["battle_id"] = ""
        self.role_to_character_leader = {}  # 存储所有的pve成员leader信息
        self.init_role_leader()  # 初始化所有角色的形象代理id

    def init_role_leader(self):
        for chas in self.atkteams:
            for cha in chas:
                if cha.is_leader:
                    rt = {}
                    rt["loco"] = cha.loco
                    rt["nickname"] = cha.nickname
                    rt["instance_id"] = cha.instance_id
                    rt["role_id"] = cha.role_id
                    self.role_to_character_leader[cha.role_id] = rt
        for chas in self.defenseteams:
            for cha in chas:
                if cha.is_leader:
                    rt = {}
                    rt["loco"] = True
                    rt["nickname"] = cha.nickname
                    rt["instance_id"] = cha.instance_id
                    rt["role_id"] = 0
                    self.role_to_character_leader[0] = rt

    def get_role_leader(self, role_id):
        return self.role_to_character_leader[role_id]

    def set_battle_id(self, battle_id):
        self.battle_id = battle_id

    def is_over(self):
        if len(self.atkteams) == 0:
            return 0
        if len(self.defenseteams) == 0:
            return 1
        return 2

    @staticmethod
    def get_hp_buff(battle_effect_list, country):
        hp_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 12 and buff[3] in (0, country):
                hp_buff[1] += buff[4]
            elif buff[2] == 46 and buff[3] in (0, country):
                hp_buff[0] += buff[4]
            elif buff[2] == 47 and buff[3] in (0, country):
                hp_buff[2] += buff[4]
            elif buff[2] == 50 and buff[3] in (0, country):
                hp_buff[1] += buff[4]
        return hp_buff

    @staticmethod
    def get_attack_buff(battle_effect_list, country, battle_type):
        attack_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 48 and buff[3] in (0, country):
                attack_buff[2] += buff[4]
            elif buff[2] == 51 and buff[3] in (0, country):
                attack_buff[1] += buff[4]
            elif buff[2] == 69 and buff[3] in (0, country):
                attack_buff[1] += buff[4]
            elif buff[2] == 72 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK) and buff[3] in (0, country):
                attack_buff[1] += buff[4]
        return attack_buff

    @staticmethod
    def get_defen_buff(battle_effect_list, country):
        defen_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 49 and buff[3] in (0, country):
                defen_buff[2] += buff[4]
            elif buff[2] == 52 and buff[3] in (0, country):
                defen_buff[1] += buff[4]
            elif buff[2] == 80 and buff[3] in (0, country):
                defen_buff[1] += buff[4]
        return defen_buff

    @staticmethod
    def get_effectshit_buff(battle_effect_list, country):
        effectshit_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 63 and buff[3] in (0, country):
                effectshit_buff[0] += buff[4]
        return effectshit_buff

    @staticmethod
    def get_effectsdef_buff(battle_effect_list, country):
        effectsdef_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 64 and buff[3] in (0, country):
                effectsdef_buff[0] += buff[4]
        return effectsdef_buff

    @staticmethod
    def get_speed_buff(battle_effect_list, country):
        speed_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 65 and buff[3] in (0, country):
                speed_buff[1] += buff[4]
        return speed_buff

    @staticmethod
    def get_battle_damage_buff(battle_effect_list, battle_type):
        damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 32:
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
            if buff[2] == 66:
                if buff[3] == 1:
                    damage_buff[1][0] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][0] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][0] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][0] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][0] += buff[4]
                    damage_buff[2][0] += buff[4]
                    damage_buff[3][0] += buff[4]
                    damage_buff[4][0] += buff[4]
            elif buff[2] == 67:
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
            elif buff[2] == 75 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.WILDBLOCK):
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
        return damage_buff

    @staticmethod
    def get_resist_damage_buff(battle_effect_list, battle_type):
        resist_damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 77 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK):
                if buff[3] == 1:
                    resist_damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    resist_damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    resist_damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    resist_damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    resist_damage_buff[1][1] += buff[4]
                    resist_damage_buff[2][1] += buff[4]
                    resist_damage_buff[3][1] += buff[4]
                    resist_damage_buff[4][1] += buff[4]
        return resist_damage_buff

    @staticmethod
    def get_exp_buff(battle_effect_list):
        exp_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 33:
                exp_buff[1] += buff[4]
        return exp_buff

    def add_card_buff(self, card, battle_effect_list, battle_type):
        country = card.country
        hp_buff = self.get_hp_buff(battle_effect_list, country)
        card.hp += hp_buff[2]
        card.hp *= (100 + hp_buff[1]) / 100
        card.hp += hp_buff[0]
        card.maxhp = card.hp
        card.recordhp = card.hp
        attack_buff = self.get_attack_buff(battle_effect_list, country, battle_type)
        card.attack += attack_buff[2]
        card.attack *= (100 + attack_buff[1]) / 100
        card.attack += attack_buff[0]
        defen_buff = self.get_defen_buff(battle_effect_list, country)
        card.defen += defen_buff[2]
        card.defen *= (100 + defen_buff[1]) / 100
        card.defen += defen_buff[0]
        speed_buff = self.get_speed_buff(battle_effect_list, country)
        card.speed *= (100 + speed_buff[1]) / 100
        effectshit_buff = self.get_effectshit_buff(battle_effect_list, country)
        card.effectshit += effectshit_buff[0]
        effectsdef_buff = self.get_effectsdef_buff(battle_effect_list, country)
        card.effectsdef += effectsdef_buff[0]

    @staticmethod
    def get_buff_record(battle_effect_list):
        buff_record = {
            "hp_buff": [0, 0, 0, 0],
            "attack_buff": [0, 0, 0, 0],
            "defense_buff": [0, 0, 0, 0],
        }
        for buff in battle_effect_list:
            if buff[2] == 50:
                if buff[3] == 1:
                    buff_record["hp_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["hp_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["hp_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["hp_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["hp_buff"][0] += buff[4]
                    buff_record["hp_buff"][1] += buff[4]
                    buff_record["hp_buff"][2] += buff[4]
                    buff_record["hp_buff"][3] += buff[4]
            elif buff[2] == 51:
                if buff[3] == 1:
                    buff_record["attack_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["attack_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["attack_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["attack_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["attack_buff"][0] += buff[4]
                    buff_record["attack_buff"][1] += buff[4]
                    buff_record["attack_buff"][2] += buff[4]
                    buff_record["attack_buff"][3] += buff[4]
            elif buff[2] == 52:
                if buff[3] == 1:
                    buff_record["defense_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["defense_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["defense_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["defense_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["defense_buff"][0] += buff[4]
                    buff_record["defense_buff"][1] += buff[4]
                    buff_record["defense_buff"][2] += buff[4]
                    buff_record["defense_buff"][3] += buff[4]

        return buff_record

    async def start(self, role_buff_effect, battle_type):
        self.round = 1
        effect = self.caculate_loco(self.atkteams)
        atk = self.atkteams[0] 
        # 复制车头，临时设置属性
        arr = []
        for cha in atk:
            arr.append(cha.deepcopy())
        atk = arr
        atk = self.merge_loco(atk, effect)
        
        defense = self.defenseteams.pop(0)

        battle = Battle(maxround=30)
        battle.create_side_cards(atk, 1)

        for card in battle.team[1]:
            character = card.character
            team_buff_list = character.get_team_buff_effect()
            role_buff_effect.extend(team_buff_list)

        for card in battle.team[1]:
            character_buff_list = card.character.get_character_buff_effect()
            battle_effects = role_buff_effect.copy() + character_buff_list

            self.add_card_buff(card, battle_effects, battle_type)
            card.damage_buff = self.get_battle_damage_buff(battle_effects, battle_type)
            card.resist_damage_buff = self.get_resist_damage_buff(battle_effects, battle_type)
            card.exp_buff = self.get_exp_buff(battle_effects)

        battle.create_side_cards(defense, -1)
        battle.create_team_log()
        battle.team_copy()
        self.battle = battle
        atk_buff_list = self.get_buff_record(role_buff_effect)
        await self.add_team_record_log(battle=battle, status="start", atk_buff_list=atk_buff_list)

        return {"win": 2, "log": {"stage":1}, "round": self.round}

    async def ga_round_one(self):
        if self.battle:
            self.battle.battlelog.clean()
            rt = self.battle.round_single_go()
            log = self.battle.caclute_pve_team_hp(self)
            self.round += 1
            log["atk_fatigue"] = 0
            log["def_fatigue"] = 0
            if rt != 2:
                # 计算消耗元气值
                team = self.battle.team_backup()
                fatigue = self.caculate_team_fatigue_vule(team[1], rt)
                average_fatigue_record = await self.average_fatigue(self.atkbackup, fatigue)
                await self.add_team_record_log(battle=self.battle, status="end",
                                               average_fatigue_record = average_fatigue_record)
                self.first_section_win = rt

                return {"win": rt, "log": log, "round": self.round}

            return {"win": rt, "log": log, "round": self.round, "ca_time": self.battle.battlelog.ca_time}
        else:
            raise GameException(status=182)

    def merge_loco(self, chas, arr):
        result = []
        for cha in chas:
            count = arr[3]
            cha.propertys.hp += arr[0] / 3
            cha.propertys.atk += arr[1] / count
            cha.propertys.defend += arr[2] / count
            result.append(cha)
        return result

    async def add_team_record_log(self, battle, status, atk_buff_list=None, average_fatigue_record=None):

        if self.team_record_log["battle_id"] == "" and self.battle_id != "":
            self.team_record_log["battle_id"] = self.battle_id

        team = battle.team_backup()
        atkteam = team[1]
        defteam = team[-1]
        team_log = {}

        arr = []
        atk_hp = 0
        for atk in atkteam:
            cha = atk.character
            if cha:
                rt = cha.to_battle_info()
                rt["hp"] = int(atk.hp)
                atk_hp += int(atk.hp)
                rt["role_id"] = atk.role_id
                rt["side"] = 1
                skills = []
                for skill in atk.skills:
                    skills.append({
                        "config_id": skill.config_id,
                        "level": skill.level,
                        "maxlevel": skill.maxlevel
                    })
                rt["skills"] = skills
                arr.append(rt)
        team_log["atk"] = arr
        team_log["atk_hp"] = atk_hp
        team_log["atk_buff_list"] = atk_buff_list if atk_buff_list else {}

        arr = []
        def_hp = 0
        for defend in defteam:
            cha = defend.character
            if cha:
                rt = cha.to_battle_info()
                rt["hp"] = int(defend.hp)
                def_hp += int(defend.hp)
                rt["role_id"] = defend.role_id
                rt["side"] = -1
                arr.append(rt)
        team_log["defend"] = arr
        team_log["def_hp"] = def_hp

        atk_fa, def_fa = self.record_current_fatigue_vule()  # 记录全部人员的总元气
        team_log["atk_fatigue"] = atk_fa
        team_log["def_fatigue"] = def_fa

        team_log["index"] = len(self.team_record_log["log"])
        team_log["status"] = status
        team_log["win"] = battle.check_win()

        if average_fatigue_record:
            # print("atk_fatigue_record", atk_fatigue_record)
            for role_id, records in average_fatigue_record.items():
                sum_total_fatigue = 0
                sum_lost_fatigue = 0
                sum_end_fatigue = 0
                role_win = 0
                role_lost = 0
                for config_id, record in records['detail'].items():
                    sum_total_fatigue += record['total_fatigue']
                    sum_lost_fatigue += record['lost_fatigue']
                    sum_end_fatigue += record['end_fatigue']
                    role_win = record['wins']
                    role_lost = record['lost']

                db_role = await DbRoleManager().get_async_db_role(role_id)
                role = Role(db_model=db_role)
                await role.async_read()
                records['role_id'] = role_id
                records['role_name'] = role.nickname
                records['role_avatar'] = role.avatar
                records['role_icon_frame'] = role.icon_frame
                records['total_fatigue'] = sum_total_fatigue
                records['lost_fatigue'] = sum_lost_fatigue
                records['end_fatigue'] = sum_end_fatigue
                records['wins'] = role_win
                records['lost'] = role_lost
            team_log["atk_fatigue_record"] = average_fatigue_record

        self.team_record_log["log"].append(team_log)

    def record_current_fatigue_vule(self):
        atk_current_fatigue = 0
        for atks in self.atkbackup:
            for atk in atks:
                atk_current_fatigue += atk.fatigue()

        def_current_fatigue = 0
        for defens in self.defenbackup:
            for defen in defens:
                def_current_fatigue += defen.fatigue()

        return atk_current_fatigue, def_current_fatigue

    def record_current_hp(self, battle):
        team = battle.team_backup()
        atk_hp = 0
        for atk in team[1]:
            atk_hp += atk.hp

        def_hp = 0
        for defen in team[-1]:
            def_hp += defen.hp

        return int(atk_hp), int(def_hp)

    def get_team_record_log(self):
        return self.team_record_log

    # 计算卡牌元气消耗
    def caculate_team_fatigue_vule(self, cardsteam, win):
        atktotal = 0
        for te in cardsteam:
            atk = self.get_pve_character(te.id)
            if atk:
                fatigue = atk.caculate_fatigueVule(te, 1, win == 0)
                atktotal += fatigue
                print("caculate_team_fatigue_vule:::", fatigue)
        return atktotal

    def get_pve_character(self, instance_id):
        for team in self.atkbackup:
            for atk in team:
                if atk.instance_id == instance_id:
                    return atk

        return None

    async def average_fatigue(self, characters, fatigue, basevalue=0, win=0):
        average_fatigue_record = {}
        total = 0
        for chas in characters:
            for cha in chas:
                total += cha.fatigue()

        if total == 0:
            return
            
        for chas in characters:
            for cha in chas:
                fat = cha.fatigue()
                total_fatigue = copy.copy(cha.fatigue())
                per = fat / total
                willfat = fatigue * per
                willfat -= basevalue
                fat = max(fat - willfat, 0)
                role = cha.role
                realcha = role.get_character(cha.config_id)
                realcha.set_fatigue(fat)
                await role.async_save()

                if cha.role_id not in average_fatigue_record:
                    average_fatigue_record[cha.role_id] = {}
                if 'detail' not in average_fatigue_record[cha.role_id]:
                    average_fatigue_record[cha.role_id]['detail'] = {}

                if cha.config_id not in average_fatigue_record[cha.role_id]['detail']:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id] = {
                        "config_id": cha.config_id,
                        "stratum": cha.stratum,
                        "lv": cha.level,
                        "total_fatigue": int(total_fatigue),
                        "lost_fatigue": int(willfat + basevalue),
                        "end_fatigue": int(fat),
                        "wins": 0,
                        "lost": 0}
                else:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]["lost_fatigue"] += int(willfat + basevalue)
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]["end_fatigue"] = int(fat)

                if win == 0:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]['wins'] += 1
                else:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]['lost'] += 1

        return average_fatigue_record

    def caculate_loco(self, characters):
        hp = 0
        atk = 0
        defend = 0
        count = 0
        for team in characters:
            for cha in team:
                effects = cha.effect  # effect是临时属性
                count += 1  # 记录所有武将个数不论有没有效果
                hp += cha.propertys.hp
                for ect in effects:
                    if ect[2] == 47:
                        hp += ect[4]
                    elif ect[3] == 48:
                        atk += ect[4]
                    elif ect[4] == 49:
                        defend += ect[4]
        return [hp, atk, defend, count]

    def get_leader(self, chas):
        for cha in chas:
            if cha.is_leader:
                return cha
        return None

    def pve_teams_log(self):
        # 根据实际需求返回队列成员长度
        cards = {}
        atks = self.atkbackup

        arr = []
        for team in atks:
            rt = None
            totalhp = 0
            for atk in team:
                if atk == 0 or atk == '0':
                    continue
                totalhp += atk.propertys.hp
                if atk.is_leader:
                    rt = atk.to_battle_info()
                    rt["is_leader"] = atk.loco
                    rt["role_id"] = atk.role_id
                    rt["owner_name"] = atk.nickname
                    rt["side"] = 1
            if rt:
                rt["maxhp"] = totalhp
                arr.append(rt)
        cards["atk"] = arr

        defens = self.defenbackup
        arr = []
        for team in defens:
            rt = None
            totalhp = 0
            for defen in team:
                if defen == 0 or defen == '0':
                    continue
                totalhp += defen.propertys.hp
                if defen.is_leader:
                    rt = defen.to_battle_info()
                    rt["is_leader"] = defen.loco
                    rt["role_id"] = defen.role_id
                    rt["owner_name"] = defen.nickname
                    rt["side"] = -1
            if rt:
                rt["maxhp"] = totalhp
                arr.append(rt)
        cards["def"] = arr
        cards["fatigue"] = {"atk_fatigue": 0, "def_fatigue": 0, "atk_max_fatigue": self.atk_max_fatigue,
                            "def_max_fatigue": self.def_max_fatigue}
        return {"round": self.round, "team": cards, "stage": 1, "wincamp": self.first_section_win}