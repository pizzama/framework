import shortuuid

from blueprint.blueprints.role import Role
from game.core.mods.mod_team.battle.battle import Battle
from game.models.dbrole import DbRoleManager
from game.servers.serverstatic import NORMALBATTLETYPE


class NormalBattle:
    def __init__(self, atkteams, defenseteams, is_pvp=False):
        self.atkteams = atkteams
        self.defenseteams = defenseteams
        self.is_pvp = is_pvp
        self.atk_max_fatigue, self.def_max_fatigue = self.record_current_fatigue_value()
        self.team_record_log = {}
        self.team_record_log["log"] = []
        self.team_record_log["character_exps"] = {}
        self.battle_id = shortuuid.uuid()

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
    def get_attack_buff(battle_effect_list, country):
        attack_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 48 and buff[3] in (0, country):
                attack_buff[2] += buff[4]
            elif buff[2] == 51 and buff[3] in (0, country):
                attack_buff[1] += buff[4]
        return attack_buff

    @staticmethod
    def get_defen_buff(battle_effect_list, country, battle_type):
        defen_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 49 and buff[3] in (0, country):
                defen_buff[2] += buff[4]
            elif buff[2] == 52 and buff[3] in (0, country):
                defen_buff[1] += buff[4]
            elif buff[2] == 82 and battle_type == NORMALBATTLETYPE.MININGDEFEN and buff[3] in (0, country):
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

    def get_battle_damage_buff(self, battle_effect_list):
        damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 32 and self.is_pvp is False:
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
            elif buff[2] == 66:
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
        return damage_buff

    @staticmethod
    def get_exp_buff(battle_effect_list):
        exp_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 33:
                exp_buff[1] += buff[4]
        return exp_buff

    def add_card_buff(self, card, battle_effect_list, battle_type=0):
        country = card.country
        hp_buff = self.get_hp_buff(battle_effect_list, country)
        card.hp += hp_buff[2]
        card.hp *= (100 + hp_buff[1]) / 100
        card.hp += hp_buff[0]
        card.maxhp = card.hp
        card.recordhp = card.hp
        attack_buff = self.get_attack_buff(battle_effect_list, country)
        card.attack += attack_buff[2]
        card.attack *= (100 + attack_buff[1]) / 100
        card.attack += attack_buff[0]
        defen_buff = self.get_defen_buff(battle_effect_list, country, battle_type)
        card.defen += defen_buff[2]
        card.defen *= (100 + defen_buff[1]) / 100
        card.defen += defen_buff[0]
        speed_buff = self.get_speed_buff(battle_effect_list, country)
        card.speed *= (100 + speed_buff[1]) / 100
        effectshit_buff = self.get_effectshit_buff(battle_effect_list, country)
        card.effectshit += effectshit_buff[0]
        effectsdef_buff = self.get_effectsdef_buff(battle_effect_list, country)
        card.effectsdef += effectsdef_buff[0]

    def start(self, role, atk_battle_effect_list, defen_role=None, defen_battle_effect_list=None, battle_type=0):

        battle = Battle()
        battle.create_side_cards(self.atkteams, 1)
        characters_exp = {}  # 存储所有的卡牌经验加成

        for card in battle.team[1]:
            character = card.character
            atk_team_buff = character.get_team_buff_effect()
            atk_battle_effect_list.extend(atk_team_buff)

        for card in battle.team[1]:
            character_buff_list = card.character.get_character_buff_effect()
            atk_battle_effects = atk_battle_effect_list.copy() + character_buff_list
            self.add_card_buff(card, atk_battle_effects, battle_type)
            card.damage_buff = self.get_battle_damage_buff(atk_battle_effects)
            card.exp_buff = self.get_exp_buff(atk_battle_effects)
            characters_exp[card.id] = card.exp_buff

        battle.create_side_cards(self.defenseteams, -1)
        print("defen_battle_effect_list", defen_battle_effect_list)
        if self.is_pvp and defen_battle_effect_list:
            defen_type = NORMALBATTLETYPE.MININGDEFEN if battle_type == NORMALBATTLETYPE.MINING else 0
            for card in battle.team[-1]:
                character = card.character
                character_buff = character.get_team_buff_effect()
                defen_battle_effect_list.extend(character_buff)

            for card in battle.team[-1]:
                character_buff_list = card.character.get_character_buff_effect()
                defen_battle_effects = defen_battle_effect_list.copy() + character_buff_list
                self.add_card_buff(card, defen_battle_effects, defen_type)
                card.damage_buff = self.get_battle_damage_buff(defen_battle_effects)
                card.exp_buff = self.get_exp_buff(defen_battle_effects)

        team_cards = battle.team_copy()
        self.add_team_record_log(card_teams=team_cards, status="start", win=2,
                                 atk_buff_list=atk_battle_effect_list,
                                 def_buff_list=defen_battle_effect_list)

        iswin = battle.start()
        win = 0 if iswin else 1

        # 记录攻击方的疲劳值
        if not self.is_pvp:
            cards = team_cards[1]
            atk_fatigue_record, _ = self.deal_character_fatigue(cards, role, iswin)
            self.add_team_record_log(
                card_teams=team_cards, status="end", win=win, atk_fatigue_record=atk_fatigue_record
            )
        else:
            atk_cards = team_cards[1]
            defen_cards = team_cards[-1]
            atk_fatigue_record, def_fatigue_record = self.deal_character_fatigue(atk_cards, role, iswin, defen_cards, defen_role)
            self.add_team_record_log(
                card_teams=team_cards, status="end", win=win,
                atk_fatigue_record=atk_fatigue_record, def_fatigue_record=def_fatigue_record
            )

        rt = {
            "team": battle.to_team_log(),
            "log": battle.to_log(),
            "win": iswin,
            "exp": characters_exp
        }

        return rt

    def record_current_fatigue_value(self):
        atk_current_fatigue = 0
        print("record_current_fatigue_value", self.atkteams)
        for atk in self.atkteams:
            atk_current_fatigue += atk.fatigue()

        def_current_fatigue = 0
        print("record_current_fatigue_value", self.defenseteams)
        for defen in self.defenseteams:
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

    def add_team_record_log(self, card_teams, status, win,
                            atk_buff_list=None, def_buff_list=None,
                            atk_fatigue_record=None, def_fatigue_record=None):  # 战报记录, status战斗前还是战斗后

        if not self.team_record_log.get("battle_id") and self.battle_id != "":
            self.team_record_log["battle_id"] = self.battle_id

        atkteam = card_teams[1]
        defteam = card_teams[-1]

        team_log = {}

        arr = []
        atk_hp = 0
        atk_fatigue = 0
        for atk in atkteam:
            cha = atk.character
            if cha:
                rt = cha.to_battle_info()
                atk_fatigue += cha.propertys.fatigueVule
                rt["hp"] = float(atk.hp)
                atk_hp += float(atk.hp)
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
        team_log["atk_fatigue"] = atk_fatigue
        team_log["atk_buff_list"] = self.get_buff_record(atk_buff_list) if atk_buff_list else {}

        arr = []
        def_hp = 0
        def_fatigue = 0
        for defend in defteam:
            cha = defend.character
            if cha:
                rt = cha.to_battle_info()
                def_fatigue += cha.propertys.fatigueVule
                rt["hp"] = float(defend.hp)
                def_hp += float(defend.hp)
                rt["role_id"] = defend.role_id
                rt["side"] = -1
                skills = []
                for skill in defend.skills:
                    skills.append({
                        "config_id": skill.config_id,
                        "level": skill.level,
                        "maxlevel": skill.maxlevel
                    })
                rt["skills"] = skills
                arr.append(rt)
        team_log["defend"] = arr
        team_log["def_hp"] = def_hp
        team_log["def_fatigue"] = def_fatigue
        team_log["def_buff_list"] = self.get_buff_record(def_buff_list) if def_buff_list else {}

        team_log["win"] = win
        team_log["status"] = status

        if atk_fatigue_record:
            print("atk_fatigue_record", atk_fatigue_record)
            for role_id, records in atk_fatigue_record.items():
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

                db_role = DbRoleManager().get_db_role(role_id)
                role = Role(db_model=db_role)
                role.read()
                records['role_id'] = role_id
                records['role_name'] = role.nickname
                records['role_avatar'] = role.avatar
                records['role_icon_frame'] = role.icon_frame
                records['total_fatigue'] = sum_total_fatigue
                records['lost_fatigue'] = sum_lost_fatigue
                records['end_fatigue'] = sum_end_fatigue
                records['wins'] = role_win
                records['lost'] = role_lost
            team_log["atk_fatigue_record"] = atk_fatigue_record

        if def_fatigue_record:
            print("def_fatigue_record", def_fatigue_record)
            for role_id, records in def_fatigue_record.items():
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

                db_role = DbRoleManager().get_db_role(role_id)
                role = Role(db_model=db_role)
                role.read()
                records['role_id'] = role_id
                records['role_name'] = role.nickname
                records['role_avatar'] = role.avatar
                records['role_icon_frame'] = role.icon_frame
                records['total_fatigue'] = sum_total_fatigue
                records['lost_fatigue'] = sum_lost_fatigue
                records['end_fatigue'] = sum_end_fatigue
                records['wins'] = role_win
                records['lost'] = role_lost
            team_log["def_fatigue_record"] = def_fatigue_record

        print("normal_battle_log", team_log)
        self.team_record_log["log"].append(team_log)

    # 记录单个卡牌元气
    def deal_character_fatigue(self, atk_cards, role, win, def_cards=None, def_role=None):

        atk_fatigue_record = {}
        win_bool = 0 if win else 1

        for atk_card in atk_cards:
            cha = role.get_character(atk_card.config_id)
            if cha:
                start_fatigue, fatigue, end_fatigue = cha.caculate_fatigueVule_and_change_character(atk_card, 1, win, 1)
                atk_card.character.caculate_fatigueVule_and_change_character(atk_card, 1, win, 1)

                if role.role_id not in atk_fatigue_record:
                    atk_fatigue_record[role.role_id] = {}
                if 'detail' not in atk_fatigue_record[role.role_id]:
                    atk_fatigue_record[role.role_id]['detail'] = {}

                if atk_card.config_id not in atk_fatigue_record[role.role_id]['detail']:
                    atk_fatigue_record[role.role_id]['detail'][atk_card.config_id] = {
                        "config_id": atk_card.config_id,
                        "stratum": atk_card.stratum,
                        "lv": atk_card.level,
                        "total_fatigue": int(start_fatigue),
                        "lost_fatigue": int(fatigue),
                        "end_fatigue": int(end_fatigue),
                        "wins": 0,
                        "lost": 0}
                else:
                    atk_fatigue_record[role.role_id]['detail'][cha.config_id]["lost_fatigue"] += int(fatigue)
                    atk_fatigue_record[role.role_id]['detail'][cha.config_id]["end_fatigue"] = int(end_fatigue)

                if win_bool == 0:
                    atk_fatigue_record[role.role_id]['detail'][cha.config_id]['wins'] += 1
                else:
                    atk_fatigue_record[role.role_id]['detail'][cha.config_id]['lost'] += 1

        def_fatigue_record = {}

        if def_cards and def_role:

            for def_card in def_cards:
                cha = def_role.get_character(def_card.config_id)
                if cha:
                    start_fatigue, fatigue, end_fatigue = cha.caculate_fatigueVule_and_change_character(def_card, 1, win, 1)
                    def_card.character.caculate_fatigueVule_and_change_character(def_card, 1, win, 1)

                    if def_role.role_id not in def_fatigue_record:
                        def_fatigue_record[def_role.role_id] = {}
                    if 'detail' not in def_fatigue_record[def_role.role_id]:
                        def_fatigue_record[def_role.role_id]['detail'] = {}

                    if def_card.config_id not in def_fatigue_record[def_role.role_id]['detail']:
                        def_fatigue_record[def_role.role_id]['detail'][def_card.config_id] = {
                            "config_id": def_card.config_id,
                            "stratum": def_card.stratum,
                            "lv": def_card.level,
                            "total_fatigue": int(start_fatigue),
                            "lost_fatigue": int(fatigue),
                            "end_fatigue": int(end_fatigue),
                            "wins": 0,
                            "lost": 0}
                    else:
                        def_fatigue_record[def_role.role_id]['detail'][def_card.config_id]["lost_fatigue"] += int(fatigue)
                        def_fatigue_record[def_role.role_id]['detail'][def_card.config_id]["end_fatigue"] = int(end_fatigue)

                    if win_bool != 0:
                        def_fatigue_record[def_role.role_id]['detail'][def_card.config_id]['wins'] += 1
                    else:
                        def_fatigue_record[def_role.role_id]['detail'][def_card.config_id]['lost'] += 1

        return atk_fatigue_record, def_fatigue_record
