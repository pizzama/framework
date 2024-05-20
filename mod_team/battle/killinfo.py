class KillInfo:
    def __init__(self) -> None:
        self.role_id = 0
        self.fatigue = 0
        self.total = 0
        self.stratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}  # 击杀精力值分类

    def set_total(self, total):
        self.total = total

    def add_num(self, stratum, num):
        currentnum = self.stratum.get(stratum)
        if not currentnum:
            currentnum = 0
        currentnum += num
        self.stratum[stratum] = currentnum

    def add_percent(self, stratum, num):
        if self.total <= 0:
            self.total = 1
        percent = self.fatigue / self.total
        num = (percent + 1) * num
        self.add_num(stratum, num)


class KillInfos:
    def __init__(self) -> None:
        self.kill = {}

    def add_team(self, team):
        total = 0
        for chas in team:
            for cha in chas:
                total += cha.fatigue()
                kill = self.kill.get(cha.role_id)
                if not kill:
                    kill = KillInfo()
                    kill.role_id = cha.role_id
                    kill.fatigue += cha.fatigue()
                    self.kill[kill.role_id] = kill

        for rid in self.kill:
            kill = self.kill[rid]
            kill.set_total(total)

    def update_percent_kill_except_roleids(self, role_ids, stratum):
        for rid in role_ids:
            kill = self.kill[rid]
            if kill:
                for k in stratum:
                    v = stratum[k]
                    kill.add_percent(k, v)

    def update_kill(self, role_ids, stratum):
        for role_id in role_ids:
            kill = self.kill.get(role_id)
            if kill:
                for k in stratum:
                    v = stratum[k]
                    kill.add_num(k, v)

    def get_kill(self, role_id):
        return self.kill.get(role_id)
