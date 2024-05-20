from skills.script import Script


class skill_5001251(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_max_property("hp")

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,4.5,4.5,4.5,4.5,4.5]
        treat=[0.05,0.1,0.15,0.2,0.25]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            # 增加特殊标记
            value = en.get_extraparams("suohun")
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*hurt
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

         # 为自己恢复血量
        hp = -self.master.attack*treat[level]*value
        self.master.hurt(self,hp)
        skldata.add_des(self.master)
        skldata.add_hurt(hp)
        
        # 创建技能日志
        self.create_skill_log(skldata)


