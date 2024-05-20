from skills.script import Script


class sk0003(Script):
    def execute(self):

        # 随机给一个友军增加出手速度
        heros = self.heros_random_target(1)
        for hero in heros:
            buf = self.create_buff(self.master, "buf_speed", {"speed": 10, "count": 2})

            rt = buf.is_effect(self.master, hero)
            if rt:
                self.skill_buff(hero, buf)

        self.create_skill_log([], [])
