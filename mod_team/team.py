class Team:
    def __init__(self):
        self.teams = []

    def setIndex(self, index, team):
        if index and len(self.teams) >= index:
            self.teams[index] = team

        if len(self.teams) < 5:
            self.teams.append(team)

    def getIndex(self, index):
        if not index:
            if len(self.teams) >= index:
                return self.teams[index]
        return None

    def deleteIndex(self, index):
        if index and len(self.teams) >= index:
            te = self.teams[index]
            self.teams.remove(te)

    def serialize(self):
        return self.teams

    def unserialize(self, value):
        self.teams = value
        if not self.teams:
            self.teams = []
