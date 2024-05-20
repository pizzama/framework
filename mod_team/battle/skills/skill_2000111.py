from skills.script import Script


class skill_2000111(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()


        # 产生伤害
        level = self.skill.level
        level4=self.get_my_skill_level("200014")
        skldata = self.create_skl_data()
        damage = [0,1.1,1.2,1.3,1.4,1.5]
        danders = [0,2,4,6,8,10]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)          
            hurt = damage[level]*hurt
            real_hurt = en.hurt(self,hurt)
            maxdander = danders[level4]
            self.master.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
            
            teams = self.heros_all()
            for team in teams:
                # 给队友添加效果，让他们接下来的攻击目标变更为对面的特定某个人
                buf = self.create_buff(self.master, "buf_taunt", {
                    "buff_id":"1200111",
                    "value":en,
                })
                
                self.skill_buff(team, buf)

            

        # 创建技能日志
        self.create_skill_log(skldata)
