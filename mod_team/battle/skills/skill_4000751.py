from skills.script import Script


class skill_4000751(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)
        hp=0
        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,2.5,2.5,2.5,2.5,2.5]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self, hurt)
            hp = hp - real_hurt *0.2
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        #恢复自身血量
        self.master.hurt(self,hp)
        skldata.add_des(self.master)
        skldata.add_hurt(hp)
        
        

        # 创建技能日志
        self.create_skill_log(skldata)
