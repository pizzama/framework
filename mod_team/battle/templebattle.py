import json

from game.exceptions import GameException
from game.core.mods.mod_team.battle.battle import Battle


class TempleBattle:
    def __init__(self, atkteams, defenseteams):
        self.atkteams = atkteams
        self.defenseteams = defenseteams
        self.atk_max_fatigue, self.def_max_fatigue = self.record_current_fatigue_value()
        self.team_record_log = {}
        self.team_record_log["battle_id"] = ""
        self.team_record_log["log"] = []
        self.battle_id = ""

    def get_atk_teams_hp(self, role_buff_effect):
        hp = 0
        for team in self.atkteams:
            team_buff_effect = role_buff_effect.copy()
            for ta in team:
                if ta:
                    team_buff_effect += ta.get_team_buff_effect()
            for ta in team:
                if ta:
                    character_buff_effect = ta.get_character_buff_effect()
                    team_buffs = team_buff_effect.copy() + character_buff_effect
                    country = ta.get_country()
                    hp_buff = [0, 0, 0]
                    for buff in team_buffs:
                        if buff[2] == 12 and buff[3] in (0, country):
                            hp_buff[1] += buff[4]
                        elif buff[2] == 46 and buff[3] in (0, country):
                            hp_buff[0] += buff[4]
                        elif buff[2] == 47 and buff[3] in (0, country):
                            hp_buff[2] += buff[4]
                        elif buff[2] == 50 and buff[3] in (0, country):
                            hp_buff[1] += buff[4]
                    _hp = ta.get_hp()
                    _hp += hp_buff[2]
                    _hp *= (100 + hp_buff[1]) / 100
                    _hp += hp_buff[0]
                    hp += _hp
        return hp

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
    def get_defen_buff(battle_effect_list, country):
        defen_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 49 and buff[3] in (0, country):
                defen_buff[2] += buff[4]
            elif buff[2] == 52 and buff[3] in (0, country):
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
    def get_battle_damage_buff(battle_effect_list):
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
        return damage_buff

    @staticmethod
    def get_exp_buff(battle_effect_list):
        exp_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 33:
                exp_buff[1] += buff[4]
        return exp_buff

    async def async_add_card_buff(self, card, battle_effect_list):
        country = card.country
        hp_buff = self.get_hp_buff(battle_effect_list, country)
        card.hp += hp_buff[2]
        card.hp *= (100 + hp_buff[1]) / 100
        card.hp += hp_buff[0]
        card.maxhp = card.hp
        attack_buff = self.get_attack_buff(battle_effect_list, country)
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

    def check_avaliabel_member(self, teams):
        count = 0
        for ta in teams:
            if ta:
                count += 1
        return count

    def set_battle_id(self, battle_id):
        self.battle_id = battle_id

    async def async_start(self, atk_roles, smulator=False):
        # 战斗
        arr = []
        atk = None
        defense = None
        self.totalround = 0
        # self.maxhp = self.get_atk_teams_hp()

        count = self.check_avaliabel_member(self.atkteams)
        if count == 0:
            raise GameException(status=117)

        # 总共多少波次怪物
        total_enemies = len(self.defenseteams)

        battle = Battle()
        while True:

            if atk is None:
                if len(self.atkteams) == 0:
                    win = False
                    if len(self.defenseteams) != 0:
                        self.add_not_fight_teams(atkteams=[], defteams=self.defenseteams)
                    break
                atk = self.atkteams.pop(0)
                atk_role_id = atk[0].role_id
                atk_role = atk_roles.get(atk_role_id)
                role_buff_effect = await atk_role.get_async_buff_effect()

                self.lefthp = self.get_atk_teams_hp(role_buff_effect)
                battle.create_side_cards(atk, 1)

                # 遍历队列，获得character上的buff
                battle_effect_list = role_buff_effect
                for card in battle.team[1]:
                    character = card.character
                    character_buff = character.get_team_buff_effect()
                    battle_effect_list.extend(character_buff)

                for card in battle.team[1]:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = battle_effect_list.copy() + character_buff_list
                    await self.async_add_card_buff(card, battle_effects)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects)

            if defense is None:
                if len(self.defenseteams) == 0:
                    win = True
                    if len(self.atkteams) != 0:
                        self.add_not_fight_teams(atkteams=self.atkteams, defteams=[])
                    break
                defense = self.defenseteams.pop(0)
                battle.create_side_cards(defense, -1)

            # team_cards = battle.team_copy()
            self.add_team_record_log(battle, "start", battle_effect_list)
            iswin = battle.start()
            # cards = team_cards[1]
            # atk_fatigue_record, _ = self.deal_character_fatigue(cards, role, iswin)
            self.add_team_record_log(battle, "end")

            if iswin:
                defense = None
                battle.reset_side_property(1)
            else:
                atk = None
                battle.reset_side_property(-1)

            rt = {
                "team": battle.to_team_log(),
                "log": battle.to_log(),
                "enemyindex": total_enemies - len(self.defenseteams)
            }
            if smulator:
                rt["slog"] = battle.to_slog()
            arr.append(rt)
            battle.reset()

        # 结算
        data = {}
        data["log"] = json.dumps(arr)
        data["win"] = win
        data["battle_id"] = self.battle_id
        return data

    def record_current_fatigue_value(self):
        atk_current_fatigue = 0
        for atks in self.atkteams:
            for atk in atks:
                atk_current_fatigue += atk.fatigue()

        def_current_fatigue = 0
        for defens in self.defenseteams:
            for defen in defens:
                def_current_fatigue += defen.fatigue()

        return atk_current_fatigue, def_current_fatigue

    def record_current_hp(self):
        atk_hp = 0
        for atks in self.atkteams:
            for atk in atks:
                atk_hp += atk.get_hp()

        def_hp = 0
        for defens in self.defenseteams:
            for defen in defens:
                def_hp += defen.get_hp()

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

    def add_team_record_log(self, battle, status, atk_buff_list=None):  # 战报记录, status战斗前还是战斗后

        if not self.team_record_log.get("battle_id") and self.battle_id != "":
            self.team_record_log["battle_id"] = self.battle_id

        team = battle.team
        atkteam = team[1]
        defteam = team[-1]
        team_log = {}

        arr = []
        atk_hp = 0
        atk_fatigue = 0
        for atk in atkteam:
            cha = atk.character
            if cha:
                rt = cha.to_battle_info()
                atk_fatigue += cha.propertys.fatigueVule
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
        team_log["atk_fatigue"] = atk_fatigue
        team_log["atk_buff_list"] = self.get_buff_record(atk_buff_list) if atk_buff_list else {}

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

        team_log["index"] = len(self.team_record_log["log"])
        team_log["status"] = status
        team_log["win"] = battle.check_win()

        self.team_record_log["log"].append(team_log)

    def add_not_fight_teams(self, atkteams, defteams):

        self.team_record_log["not_fight_atk_teams"] = []
        self.team_record_log["not_fight_def_teams"] = []

        if atkteams:
            for atkteam in atkteams:
                arr = []
                for atk_char in atkteam:
                    rt = atk_char.to_battle_info()
                    rt["hp"] = float(int(atk_char.propertys.hp))
                    rt["role_id"] = atk_char.role_id
                    rt["side"] = 1
                    arr.append(rt)
                self.team_record_log["not_fight_atk_teams"].append(arr)

        if defteams:
            for defteam in defteams:
                arr = []
                for enemy in defteam:
                    rt = enemy.to_battle_info()
                    rt["hp"] = float(int(enemy.propertys.hp))
                    rt["role_id"] = 0
                    rt["side"] = -1
                    arr.append(rt)
                self.team_record_log["not_fight_def_teams"].append(arr)

    # 记录单个卡牌元气
    def deal_character_fatigue(self, atk_cards, role, win):

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

        return atk_fatigue_record
