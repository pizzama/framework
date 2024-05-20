import json


class BattleLog:
    def __init__(self):
        pass

    def skill_log(self, script, master, skldata):
        arr = []
        enemys = skldata.get_des()
        for i in enemys:
            arr.append(i.id)

        flag = "skl"
        params = {
            'des': arr,
            'htp': skldata.get_htp(),
            'skill_index': script.get_skill_type(),  # 0 普攻, 1 默认值
            'skill_id': script.sid,
            'hps': [],
            'hurt': skldata.get_hurt(),
            'src': master.id,
            'eng': skldata.eng,
            'name': "",
            'skill_name': "",
        }
        return {'flag': flag, 'params': params}

    def rst_log(self, rd):
        flag = "rst"
        params = {"no": rd}

        return {'flag': flag, 'params': params}

    def lck_log(self, uid):
        flag = "lck"
        params = {"src": uid}

        return {'flag': flag, 'params': params}

    def rls_log(self, uid):
        flag = "rls"
        params = {"src": uid}

        return {'flag': flag, 'params': params}

    def mkb_log(self, srcid, desid, buff_id):
        flag = "mkb"
        params = {'des': desid, 'effid': buff_id, 'addtype': 0, 'insid': buff_id, 'name': "", 'src': srcid}

        return {'flag': flag, 'params': params}

    def bck_log(self, srcarr, desarr, hurtarr, buff_id):
        flag = "bck"
        params = {'src': srcarr, 'des': desarr, 'hurt': hurtarr, 'insid': buff_id, 'effid': buff_id}

        return {'flag': flag, 'params': params}

    def rmb_log(self, srcid, desid, buff_id):
        flag = "rmb"
        params = {'des': desid, 'effid': buff_id, 'insid': buff_id, 'name': "", 'src': srcid}

        return {'flag': flag, 'params': params}

    def death_log(self, src):
        flag = "dth"
        params = {'src': src.id}

        return {'flag': flag, 'params': params}

    def win_log(self, result):
        flag = "winer"
        params = {"win": result}
        return {'flag': flag, 'params': params}

    def mission_log(self, result):
        flag = "cond"
        params = result
        return {'flag': flag, "params": params}

    def simulator_round(self, round):
        return ('[战斗第%s回合]' % round)

    def simulator_skill(self, script, master, skldata):
        enemys = skldata.get_des()
        hurts = skldata.get_hurt()
        template = '[%s 使用技能 %s 攻击了 %s 分别产生了 %s]'

        ens = ""
        for enemy in enemys:
            ens = ens + enemy.get_card_name() + ","

        hs = ""
        for hurt in hurts:
            hs = hs + str(hurt) + ","

        rt = template % (master.get_card_name(), script.sid, ens, hs)

        return rt

    def simulator_mkb(self, src, master, buffid, buffcount):
        template = '[%s 给 %s 添加了 %s 的buff持续 %s 回合]'
        rt = template % (src.get_card_name(), master.get_card_name(), buffid, buffcount)
        return rt

    def simulator_rmb(self, src, master, buffid, buffcount):
        template = '[%s 给 %s 移除了 %s 的buff]'
        rt = template % (src.get_card_name(), master.get_card_name(), buffid)
        return rt

    def simulator_bck(self, srcs, targets, hurts, buff_id):
        template = '[%s buff %s 对 %s 分别造成了 %s 的伤害]'
        rt = template % (str(srcs), buff_id, str(targets), str(hurts))
        return rt

    def simulator_death(self, src):
        template = '[%s 死亡了]'
        rt = template % src.get_card_name()
        return rt


class HurtLog:
    def __init__(self) -> None:
        self.side = 0
        self.skill_hurt = 0
        self.normal_hurt = 0
        self.recover_hp = 0

    def add_log(self, side, hurt, hurt_type):  # hurt_type 1: 普通攻击, 2: 技能攻击S
        self.side = side
        if hurt < 0:
            self.recover_hp += hurt
        if hurt_type == 1:
            self.normal_hurt += hurt
        elif hurt_type == 2:
            self.skill_hurt += hurt

    def __add__(self, hurt_log):
        log = HurtLog()
        log.skill_hurt = self.skill_hurt + hurt_log.skill_hurt
        log.normal_hurt = self.normal_hurt + hurt_log.normal_hurt
        log.recover_hp = self.recover_hp + hurt_log.recover_hp
        return log

    def to_dict(self):
        return {"hurts": [self.skill_hurt, self.normal_hurt, self.recover_hp], "htps": [1, 0, 2]}


class BattleLogManager:
    def __init__(self):
        self.battle_logs = []  # 战斗日志
        self.team_logs = []  # team 阵容logs
        self.simulate_logs = []  # 模拟战斗日志
        self.ca_time = 0  # 每回合大概时间
        self.hurt_logs = []  # 伤害日志

    def clean(self):
        self.battle_logs = []  # 战斗日志
        self.team_logs = []  # team 阵容logs
        self.simulate_logs = []  # 模拟战斗日志
        self.ca_time = 0  # 每回合大概时间
        self.hurt_logs = []  # 伤害日志

    def caculate_time(self, time):
        if self.ca_time is None:
            self.ca_time = 0
        self.ca_time = max(self.ca_time, time)

    def round_record(self, round):
        self.battle_logs.append(BattleLog().rst_log(round))
        self.simulate_logs.append(BattleLog().simulator_round(round))

    def master_record(self, heros):
        arr = []
        for hero in heros:
            arr.append(json.dumps(hero.serialize()))
        # 每一回合显示角色的基本属性
        self.simulate_logs.append(arr)

    def lck_record(self, id):
        self.battle_logs.append(BattleLog().lck_log(id))

    def rls_record(self, id):
        self.battle_logs.append(BattleLog().rls_log(id))

    def skill_record(self, script, master, skldata):
        self.battle_logs.append(BattleLog().skill_log(script, master, skldata))

        self.simulate_logs.append(BattleLog().simulator_skill(script, master, skldata))

    def buff_record(self, buff):
        self.battle_logs.append(BattleLog().mkb_log(buff.src.id, buff.target.id, buff.buff_id))
        self.simulate_logs.append(BattleLog().simulator_mkb(buff.src, buff.target, buff.get_name(), buff.count))

    def buff_action_record(self, buff, srcs, targets, hurts):
        # todo为了解决buff的攻击者
        self.battle_logs.append(BattleLog().bck_log(targets[0], targets, hurts, buff.buff_id))
        # self.battle_logs.append(BattleLog().bck_log(srcs, targets, hurts, buff.buff_id))
        self.simulate_logs.append(BattleLog().simulator_bck(srcs, targets, hurts, buff.buff_id))

    def buff_only_record(self, buff, srcs, targets, hurts):
        self.simulate_logs.append(BattleLog().simulator_bck(srcs, targets, hurts, buff.buff_id))
    def buff_del_record(self, buff):
        self.battle_logs.append(BattleLog().rmb_log(buff.src.id, buff.target.id, buff.buff_id))
        self.simulate_logs.append(BattleLog().simulator_rmb(buff.src, buff.target, buff.get_name(), buff.count))

    def death_record(self, src):
        self.battle_logs.append(BattleLog().death_log(src))
        self.simulate_logs.append(BattleLog().simulator_death(src))

    def win_log(self, result):
        self.battle_logs.append(BattleLog().win_log(result))

    def mission_log(self, result):
        self.battle_logs.append(BattleLog().mission_log(result))

    def result_record(self, rt):
        if rt == 0:
            self.win_log(True)
            self.simulate_logs.append("[攻击方胜利]")
            print("atk success")
            return True
        else:
            self.win_log(False)
            self.simulate_logs.append("[防守方胜利]")
            print("def success")
            return False

    def to_log(self):
        return self.battle_logs

    def to_slog(self):
        return self.simulate_logs

    def make_team_log(self, atkteam, defteam):
        self.team_logs = {}
        for _, ta in enumerate(atkteam):
            if ta:
                rt = ta.serialize()
                rt['index'] = ta.index + 1
                rt['side'] = 1
                self.team_logs[ta.id] = rt
                # print("atk:", rt)

        for _, ta in enumerate(defteam):
            if ta:
                rt = ta.serialize()
                rt['index'] = ta.index + 1
                rt['side'] = -1
                self.team_logs[ta.id] = rt
                # print("def:", rt)

    def to_team_log(self):
        return self.team_logs

    def record_hurt_log(self, skldata, hurt_type):
        des = skldata.get_des()
        hurts = skldata.get_hurt()
        for index, target in enumerate(des):
            hurt_log = HurtLog()
            hurt_log.add_log(target.side, hurts[index], hurt_type)
            self.hurt_logs.append(hurt_log)

    def caclute_all_hurt_log(self):
        atk_log = HurtLog()
        def_log = HurtLog()
        for hurt_log in self.hurt_logs:
            if hurt_log.side == 1:
                atk_log += hurt_log
            elif hurt_log.side == -1:
                def_log += hurt_log
        return {-1: def_log, 1: atk_log}

    def caclute_team_hp(self, battle, pvp):
        atkfatigue = 0
        deffatigue = 0
        atks = battle.team[1]
        defens = battle.team[-1]
        atkups = battle.team_backup()[1]
        defensups = battle.team_backup()[-1]
        if len(atks) == 0:
            for at in atkups:
                at.hp = 0
        if len(defens) == 0:
            for de in defensups:
                de.hp = 0


        role_id = atkups[0].role_id
        atkleader = pvp.get_role_leader(role_id)
        role_id = defensups[0].role_id
        defensleader = pvp.get_role_leader(role_id)

        if pvp:
            for aaa in atkups:
                cha = pvp.get_pvp_character(aaa.id, 1)
                if cha:
                    fatigue = cha.fatigue()
                    print("caclute_team_hp atk:", cha.is_leader, cha.fatigue(),cha.instance_id)
                    if cha.is_leader and fatigue <= 0:
                        atkfatigue = 0
                        break
                    atkfatigue += cha.fatigue()
            for ddd in defensups:
                cha = pvp.get_pvp_character(ddd.id, -1)
                if cha:
                    fatigue = cha.fatigue()
                    print("caclute_team_hp def:", cha.is_leader, cha.fatigue(),cha.instance_id)
                    if cha.is_leader and fatigue <= 0:
                        deffatigue = 0
                        break
                    deffatigue += fatigue

        log = battle.battlelog.caclute_all_hurt_log()
        atk_hurt_log = log[1].to_dict()
        atk_hurt_log.update({"hp": 0, "maxhp": battle.totateamhp[1], "fatigue": atkfatigue, "is_leader": atkleader["loco"], "owner_name": atkleader["nickname"], "role_id": atkleader["role_id"]})
        def_hurt_log = log[-1].to_dict()
        def_hurt_log.update({"hp": 0, "maxhp": battle.totateamhp[-1], "fatigue": deffatigue, "is_leader": defensleader["loco"], "owner_name": defensleader["nickname"], "role_id": defensleader["role_id"]})

        result = {atkleader["instance_id"]: atk_hurt_log, defensleader["instance_id"]: def_hurt_log}
        for atk in atkups:
            result[atkleader["instance_id"]]["hp"] += atk.hp
        for den in defensups:
            result[defensleader["instance_id"]]["hp"] += den.hp
        return result

    def caclute_pve_team_hp(self, battle, gabattle):
        atkfatigue = 0
        deffatigue = 0
        atks = battle.team[1]
        defens = battle.team[-1]
        atkups = battle.team_backup()[1]
        defensups = battle.team_backup()[-1]
        if len(atks) == 0:
            for at in atkups:
                at.hp = 0
        if len(defens) == 0:
            for de in defensups:
                de.hp = 0


        role_id = atkups[0].role_id
        atkleader = gabattle.get_role_leader(role_id)
        role_id = defensups[0].role_id
        defensleader = gabattle.get_role_leader(role_id)

        log = battle.battlelog.caclute_all_hurt_log()
        atk_hurt_log = log[1].to_dict()
        atk_hurt_log.update({"hp": 0, "maxhp": battle.totateamhp[1], "fatigue": atkfatigue, "is_leader": atkleader["loco"], "owner_name": atkleader["nickname"], "role_id": atkleader["role_id"]})
        def_hurt_log = log[-1].to_dict()
        def_hurt_log.update({"hp": 0, "maxhp": battle.totateamhp[-1], "fatigue": deffatigue, "is_leader": defensleader["loco"], "owner_name": defensleader["nickname"], "role_id": defensleader["role_id"]})

        result = {atkleader["instance_id"]: atk_hurt_log, defensleader["instance_id"]: def_hurt_log}
        for atk in atkups:
            result[atkleader["instance_id"]]["hp"] += atk.hp
        for den in defensups:
            result[defensleader["instance_id"]]["hp"] += den.hp
        return result    
