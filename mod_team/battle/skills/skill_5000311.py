from skills.script import Script


class skill_5000311(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()
        heros = self.heros_random_target(1)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage =[0,2.0,2.5,3.0,3.5,4.0]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            # 增加特殊标记
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        for hero in heros:
            # 给己方随机一个队友加一个提升攻击力的buff
            randomatk=[0,0.1,0.15,0.2,0.25,0.3]
            buf = self.create_buff(self.master, "buf_atk", {
                "buff_id":"11001",
                "value": self.master.attack*randomatk[level],
            })

            self.skill_buff(hero, buf)

        # 创建技能日志
        self.create_skill_log(skldata)
        
        skill4level = self.get_my_skill_level("500034")
        if skill4level > 0:
           defens=[0,-0.08,-0.1,-0.12,-0.16,-0.2]
           for en in enemys:
                buf = self.create_buff(self.master, "buf_def", {
                "buff_id":"1500341",
                "value": en.defen*defens[skill4level],
                })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)
               