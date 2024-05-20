from skills.script import Script


class skill_5000211(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)
       
        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.2,1.5,1.8,2.1,2.4]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
             # 增加特殊标记
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)
        
        for en in enemys:
            # 给敌人加一个灼烧效果
            hurts=[0,0.4,0.5,0.6,0.7,0.8]
            buf = self.create_buff(self.master, "buf_hp", {
                "buff_id":"13002",
                "value": self.master.attack*hurts[level],
                "count":3,
            })
            
            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)


